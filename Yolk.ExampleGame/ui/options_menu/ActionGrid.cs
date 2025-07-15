namespace Yolk.UI.Options;

using System;
using System.Linq;
using Godot;
using Yolk.Extensions;


public partial class ActionGrid : GridContainer {
  [Export] private PackedScene _actionContainerScene = default!;
  public override void _Ready() {
    this.ClearChildren();

    var actionsToShow = InputMap.GetActions().Select(a => a.ToString())
      .Where(a => !a.StartsWith("ui_"))
      .Where(a => !a.StartsWith("hard_"))
      .Where(a => !a.StartsWith("debug_"));

    foreach (var action in actionsToShow) {
      var container = _actionContainerScene?.Instantiate<ActionBindButton>() ?? throw new MissingFieldException();

      container.Action = action;
      AddChild(container);
    }
  }
}
