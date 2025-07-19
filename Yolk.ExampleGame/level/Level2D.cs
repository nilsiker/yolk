namespace Yolk.World;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface ILevel2D : INode2D, IProvide<AudioStream> {
  public Node2D? GetEntrypointTransform(string name);
}

[Meta(typeof(IAutoNode))]
public partial class Level2D : Node2D, ILevel2D {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private AudioStream LevelMusic { get; set; } = default!;

  [Dependency] private IWorldRepo WorldRepo { get; set; } = default!;
  [Node] private Node Entrypoints { get; set; } = default!;

  AudioStream IProvide<AudioStream>.Value() => LevelMusic;

  public static string LEVEL_DIR => "res://level/scenes/";
  public static string DEBUG_LEVEL => "0_DebugBox1";
  public static string GetLevelPath(string name) => LEVEL_DIR + name + ".tscn";

  public void OnResolved() => this.Provide();

  public Node2D? GetEntrypointTransform(string name) => Entrypoints.FindChild(name) as Node2D;

}
