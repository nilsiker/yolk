namespace Yolk;

using Godot;

public partial class DebugPanel : PanelContainer {

  public override void _Ready() => Visible = false;

  public override void _Process(double delta) {
    GD.Print("debugpanel");
  }

  public override void _Input(InputEvent @event) {
    if (@event.IsActionPressed(Inputs.ToggleDebug)) {
      Visible = !Visible;
    }
  }
}
