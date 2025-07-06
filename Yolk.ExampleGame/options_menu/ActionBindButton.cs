namespace Yolk.UI.Options;

using System;
using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;
using Yolk.Controls;
using Yolk.FS;

[Meta(typeof(IAutoNode))]
public partial class ActionBindButton : Button {
  public override void _Notification(int what) => this.Notify(what);

  public string? Action {
    get; set;
  }

  private ActionContainerLogic Logic { get; set; } = default!;
  private ActionContainerLogic.IBinding Binding { get; set; } = default!;

  public void OnResolved() {
    Logic = new();
    Binding = Logic.Bind();

    Binding
      .Handle((in ActionContainerLogic.Output.BindAction output) => OnOutputBindAction(output.Action, output.Event))
      .Handle((in ActionContainerLogic.Output.UpdateIcon output) => OnOutputUpdateIcon(output.Icon));

    Logic.Set(new ActionContainerLogic.Data());

    Pressed += OnPressed;

    Text = $" {Action}" ?? "<missing action>";
  }

  private void OnOutputUpdateIcon(Texture2D? icon) {
    if (icon is null) {
      KenneyOneBitInput
        .Tile(InputMap.ActionGetEvents(Action ?? throw new MissingFieldException("no action specified"))
        .FirstOrDefault()?
        .AsText() ?? "unknown");
      return;

    }

    Icon = icon;
  }


  private void OnPressed() =>
    Logic.Input(new ActionContainerLogic.Input.Listen(Action ?? throw new MissingFieldException("no action specified")));


  private void OnOutputBindAction(string action, InputEvent @event) {
    InputMap.ActionEraseEvents(action);
    InputMap.ActionAddEvent(action, @event);

    GodotConfig.WriteMappedAction(action);
    GetViewport().SetInputAsHandled();
  }

  public override void _Input(InputEvent @event) {
    if (@event.IsReleased() || @event.IsEcho() || @event is InputEventMouseMotion) {
      return;
    }

    if (@event.IsAction(Inputs.Pause)) {
      Logic.Input(new ActionContainerLogic.Input.Cancel());
    }
    else {
      Logic.Input(new ActionContainerLogic.Input.Bind(@event));
    }
  }


  [Meta, LogicBlock(typeof(State), Diagram = true)]
  public partial class ActionContainerLogic : LogicBlock<ActionContainerLogic.State> {
    public override Transition GetInitialState() => To<State.Idle>();

    public class Data {
      public string? Action { get; set; }
    }

    public static class Input {
      public readonly record struct Listen(string Action);
      public readonly record struct Bind(InputEvent Event);
      public readonly record struct Cancel;
    }

    public static class Output {
      public readonly record struct BindAction(string Action, InputEvent Event);
      public readonly record struct UpdateIcon(Texture2D? Icon);
    }

    public abstract partial record State : StateLogic<State> {
      public partial record Idle : State, IGet<Input.Listen> {
        public Transition On(in Input.Listen input) {
          Get<Data>().Action = input.Action;
          return To<Listening>();
        }
      }

      public partial record Listening : State, IGet<Input.Cancel>, IGet<Input.Bind> {
        public Listening() {
          this.OnEnter(() => Output(new Output.UpdateIcon()));
          this.OnExit(() => Output(new Output.UpdateIcon()));
        }
        public Transition On(in Input.Cancel input) => To<Idle>();
        public Transition On(in Input.Bind input) {
          var action = Get<Data>().Action;

          if (action is not null) {
            Output(new Output.BindAction(action, input.Event));
          }
          else {
            GD.PushWarning("Action was null when in listen state");
          }

          return To<Idle>();
        }
      }
    }
  }
}


