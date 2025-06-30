namespace Yolk;

using System.Collections.Generic;
using System.Linq;
using Godot;

[SceneTree]
public partial class StateDebugDisplay : HBoxContainer {
  private IEnumerable<IStateInfo> NodesInStateGroup => GetTree().GetNodesInGroup("state").OfType<IStateInfo>();

  public override void _Ready() { }

  public override void _Process(double delta) {
    var str = "";

    foreach (var node in NodesInStateGroup) {
      str += $"{node.Name}: {node.State}\n";
    }
    _.Label.Text = str.TrimEnd();
  }
}
