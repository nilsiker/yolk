namespace Yolk;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode))]
public partial class DebugPanel : PanelContainer {
  public override void _Notification(int what) => this.Notify(what);

  [Node] private Label FPSValue { get; set; } = default!;

  public override void _Ready() => Visible = false;

  public override void _Process(double delta) => FPSValue.Text = Engine.GetFramesPerSecond().ToString();


  public override void _Input(InputEvent @event) {
    if (@event.IsActionPressed(Inputs.ToggleDebug)) {
      Visible = !Visible;
    }
  }
}
