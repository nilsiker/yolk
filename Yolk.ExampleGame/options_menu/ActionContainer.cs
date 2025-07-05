namespace Yolk.UI.Options;

using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.Controls;

[Meta(typeof(IAutoNode))]
public partial class ActionContainer : HBoxContainer {
  public override void _Notification(int what) => this.Notify(what);

  public string? Action {
    get; set;
  }

  [Dependency]
  private ControlsRepo Controls => this.DependOn<ControlsRepo>();

  [Node] private Button BindButton { get; set; } = default!;

  public void OnResolved() {
    Controls.ActionMapped += OnControlsActionMapped;

    BindButton.Text = $" {Action}    " ?? "<missing action>";
  }

  private void OnControlsActionMapped(string action, string key) {
    if (action == Action) {
      GD.Print("update icon");
    }
  }
}
