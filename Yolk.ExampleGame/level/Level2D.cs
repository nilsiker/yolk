namespace Yolk.World;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface ILevel2D : INode2D {
  public Node2D? GetEntrypointTransform(string name);
}

[Meta(typeof(IAutoNode))]
public partial class Level2D : Node2D, ILevel2D {
  public override void _Notification(int what) => this.Notify(what);
  public static string LEVEL_DIR => "res://level/scenes/";
  public static string DEBUG_LEVEL => "0_DebugBox1";
  public static string GetLevelPath(string name) => LEVEL_DIR + name + ".tscn";

  [Dependency] private IWorldRepo WorldRepo { get; set; } = default!;
  [Node] private Node Entrypoints { get; set; } = default!;

  public Node2D? GetEntrypointTransform(string name) => Entrypoints.FindChild(name) as Node2D;
}
