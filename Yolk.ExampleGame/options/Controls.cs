namespace Yolk.Controls;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Generator;
using Yolk.Logic.Controls;

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class Controls : Node {
  public override void _Notification(int what) => this.Notify(what);

  private ControlsLogic Logic { get; set; } = new();
  private ControlsLogic.IBinding Binding { get; set; } = default!;
  public ControlsRepo ControlsRepo = OS.

  public void OnResolved() {
    Binding = Logic.Bind();

    Logic.Set(new ControlsLogic.Data());

    Binding.Handle((in ControlsLogic.Output.ActionMapped output) => OnOutputActionMapped(output.Action, output.Key));
  }

  private static void OnOutputActionMapped(string action, InputEventKey key) {
    var actions = InputMap.GetActions();
    GD.Print("all actions: ", actions);

    GD.Print($"wants to map {key.AsText()} to {action}");
  }


  public override void _Input(InputEvent @event) {
    if (@event.IsPressed() && @event.IsEcho()) {
      return;
    }

    if (@event.IsActionPressed(Inputs.Rebind)) {
      Logic.Input(new ControlsLogic.Input.Listen(Inputs.Rebind, EActionType.Key));
    }
    else if (@event.IsPressed() && !@event.IsEcho() && @event is InputEventKey key) {
      Logic.Input(new ControlsLogic.Input.PressKey(key));
    }
  }
}
