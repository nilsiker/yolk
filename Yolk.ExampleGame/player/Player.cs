namespace Yolk.ExampleGame;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Godot;
using Yolk.Controls;
using Yolk.Core;
using Yolk.Data;
using Yolk.Game;
using Yolk.Generator;
using Yolk.Logic.Player;
using Yolk.Logic.World;
using Yolk.World;

public interface IPlayer : ICharacterBody2D, IKillable;

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class Player : CharacterBody2D, IPlayer {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IWorldRepo WorldRepo => this.DependOn<IWorldRepo>();
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private ISaveChunk<GameData> GameChunk => this.DependOn<ISaveChunk<GameData>>();

  void IKillable.Kill() => Logic.Input(new PlayerLogic.Input.Kill());

  private ISaveChunk<PlayerData> PlayerChunk { get; set; } = default!;
  private PlayerLogic Logic { get; set; } = default!;
  private PlayerLogic.IBinding Binding { get; set; } = default!;

  public void Setup() {
    Logic = new();

    PlayerChunk = new SaveChunk<PlayerData>(
     onSave: chunk => new PlayerData {
       Px = GlobalPosition.X,
       Py = GlobalPosition.Y,
       Logic = Logic
     },
     onLoad: (chunk, data) => {
       GlobalPosition = new(data.Px, data.Py);
       Logic.RestoreFrom(data.Logic);
     });
  }

  public void OnResolved() {
    GameChunk.OverwriteChunk(PlayerChunk);

    Binding = Logic.Bind();
    Binding
      .Handle((in PlayerLogic.Output.Teleport output) => OnOutputTeleport(output.Entrypoint))
      .Handle((in PlayerLogic.Output.SetEnabled output) => OnOutputSetEnabled(output.Enabled));

    Logic.Set(WorldRepo);
    Logic.Set(GameRepo);
    Logic.Start();
  }

  private void OnOutputSetEnabled(bool enabled) => SetCollisionLayerValue(1, enabled);

  private void OnOutputTeleport(ITransform2D entrypoint) {
    GD.Print($"Teleporting player to {entrypoint.Position}");
    GlobalPosition = new Vector2(entrypoint.Position.X, entrypoint.Position.Y);
  }

  private float _gravity;

  public override void _PhysicsProcess(double delta) {
    var inputVector = Inputs.GetMoveVector();

    Velocity = Velocity with {
      X = inputVector.X * 75,
      Y = _gravity * 10
    };

    MoveAndSlide();

    _gravity = IsOnFloor() ? 0 : _gravity + (3 * 9.82f * (float)delta);
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(Inputs.Jump)) {
      _gravity = -10;
    }
  }
  public override void _ExitTree() => Binding.Dispose();

}
