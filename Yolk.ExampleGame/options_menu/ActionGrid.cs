namespace Yolk.UI.Options;

using System;
using System.Linq;
using Godot;

public partial class ActionGrid : GridContainer {
  [Export] private PackedScene _actionContainerScene = default!;
  public override void _Ready() {
    this.ClearChildren();

    foreach (var action in InputMap.GetActions().Where(a => !a.ToString().StartsWith("ui_"))) {
      var container = _actionContainerScene?.Instantiate<ActionBindButton>() ?? throw new MissingFieldException();

      container.Action = action;
      AddChild(container);
    }
  }
}
