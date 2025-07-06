namespace Yolk.UI.Options;

using System;
using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;
using Yolk.Controls;
using Yolk.Options.Actions;


[Meta(typeof(IAutoNode))]
public partial class ActionBindButton : Button {
  public override void _Notification(int what) => this.Notify(what);

  public string Action {
    get; set;
  } = default!;

  [Dependency] private IActionRepo ActionRepo => this.DependOn<IActionRepo>();

  [Node] private Control InputBlocker { get; set; } = default!;

  private ActionContainerLogic Logic { get; set; } = default!;
  private ActionContainerLogic.IBinding Binding { get; set; } = default!;

  public void OnResolved() {
    Logic = new();
    Binding = Logic.Bind();

    Binding
      .Handle((in ActionContainerLogic.Output.UpdateInputPrompt output) => OnOutputUpdateIcon(output.InputName))
      .Handle((in ActionContainerLogic.Output.OnDefaultRestored _) => OnDefaultRestored());

    Logic.Set(ActionRepo);
    Logic.Set(new ActionContainerLogic.Data {
      Action = Action
    });

    Pressed += OnPressed;

    // Set the action to default
    if (Action is not null) {
      var input = InputMap.ActionGetEvents(Action)
            .FirstOrDefault()?
            .AsText()
            .Split(" (").FirstOrDefault() ?? "unknown";

      Text = GetButtonText(Action, input);
    }
  }

  private void OnDefaultRestored() {
    // Reset the button text to the default action
    var input = InputMap.ActionGetEvents(Action)
         .FirstOrDefault()?
         .AsText()
         .Split(" (").FirstOrDefault() ?? "unknown";

    Text = GetButtonText(Action, input);
  }


  private void OnOutputUpdateIcon(string? inputName) {
    if (string.Empty == inputName) {
      Text = $" {Action} [...]";
      return;
    }

    var input = InputMap.ActionGetEvents(Action)
         .FirstOrDefault()?
         .AsText()
         .Split(" (")
         .FirstOrDefault() ?? "unknown";

    Text = GetButtonText(Action, input);
  }


  private void OnPressed() =>
    Logic.Input(new ActionContainerLogic.Input.Listen(Action));

  private static string GetButtonText(string action, string input) => $" {action} [{input}]";

  public override void _Input(InputEvent @event) {
    if (@event.IsReleased() || @event.IsEcho() || @event is InputEventMouseMotion) {
      return;
    }

    if (@event.IsAction(Inputs.HardCancel)) {
      Logic.Input(new ActionContainerLogic.Input.Stop());
    }
    else {
      Logic.Input(new ActionContainerLogic.Input.Bind(@event));
    }
  }


  [Meta, LogicBlock(typeof(State), Diagram = true)]
  public partial class ActionContainerLogic : LogicBlock<ActionContainerLogic.State> {
    public override Transition GetInitialState() => To<State.Idle>();

    public class Data {
      public required string Action { get; set; }
    }

    public static class Input {
      public readonly record struct Listen(string Action);
      public readonly record struct Bind(InputEvent Event);
      public readonly record struct Stop;
    }

    public static class Output {
      public readonly record struct UpdateInputPrompt(string InputName);
      public readonly record struct OnDefaultRestored;
    }

    public abstract partial record State : StateLogic<State> {
      public State() {
        OnAttach(() => Get<IActionRepo>().DefaultsRestored += OnDefaultsRestored);
        OnDetach(() => Get<IActionRepo>().DefaultsRestored -= OnDefaultsRestored);
      }

      private void OnDefaultsRestored() => Output(new Output.OnDefaultRestored());

      public partial record Idle : State, IGet<Input.Listen> {
        public Transition On(in Input.Listen input) => To<Listening>();
      }

      public partial record Listening : State, IGet<Input.Stop>, IGet<Input.Bind> {
        public Listening() {
          OnAttach(() => Get<IActionRepo>().ActionMapped += OnActionMapped);
          OnDetach(() => Get<IActionRepo>().ActionMapped -= OnActionMapped);

          this.OnEnter(() => Output(new Output.UpdateInputPrompt("")));
          this.OnExit(() => Output(new Output.UpdateInputPrompt()));
        }

        private void OnActionMapped(string arg1, InputEvent @event) {
          if (Get<Data>().Action == arg1) {
            Output(new Output.UpdateInputPrompt(@event.AsText() ?? "unknown"));
            Input(new Input.Stop());
          }
        }

        public Transition On(in Input.Stop input) => To<Idle>();
        public Transition On(in Input.Bind input) {
          var action = Get<Data>().Action;

          Get<IActionRepo>().MapAction(action, input.Event);

          return ToSelf();
        }
      }
    }
  }
}


