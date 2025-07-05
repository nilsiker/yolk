namespace Yolk.ExampleGame;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Generator;
using Yolk.Logic.Player;
using Yolk.Logic.World;
using Yolk.World;

public interface IPlayer : ICharacterBody2D;

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class Player : CharacterBody2D, IPlayer {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IWorldRepo WorldRepo => this.DependOn<IWorldRepo>();

  private PlayerLogic Logic { get; set; } = default!;
  private PlayerLogic.IBinding Binding { get; set; } = default!;

  public void OnResolved() {
    Logic = new();
    Binding = Logic.Bind();

    Binding.Handle((in PlayerLogic.Output.Teleport output) => OnOutputTeleport(output.Entrypoint));

    Logic.Set(WorldRepo);
    Logic.Start();
  }

  private void OnOutputTeleport(ITransform2D entrypoint) {
    GlobalPosition = new Vector2(entrypoint.Position.X, entrypoint.Position.Y);
    GlobalRotation = entrypoint.Rotation;
  }

  public override void _ExitTree() => Binding.Dispose();
}
