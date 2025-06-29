namespace Yolk.World;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.World;

public interface ILevel : INode3D {
  public Node3D? GetLanding(string name);
}

[Meta(typeof(IAutoNode))]
public partial class Level : Node3D, ILevel {
  public override void _Notification(int what) => this.Notify(what);
  public static string LEVEL_DIR => "res://src/level/scenes/";
  public static string DEBUG_LEVEL => "0_DebugBox1";
  public static string GetLevelPath(string name) => LEVEL_DIR + name + ".tscn";

  [Dependency] private IWorldRepo WorldRepo { get; set; } = default!;
  [Node] private Node Landings { get; set; } = default!;


  public Node3D? GetLanding(string name) => Landings.FindChild(name) as Node3D;
}