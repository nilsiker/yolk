namespace Yolk.World;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IWorldProvider : INode, IProvide<IWorldRepo>;

[Meta(typeof(IAutoNode))]
public partial class WorldProvider : Node, IWorldProvider {
  private WorldRepo WorldRepo { get; set; } = new();

  public IWorldRepo Value() => WorldRepo;

  public void OnResolved() => this.Provide();
}
