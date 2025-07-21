namespace Yolk.ExampleGame;

using System;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Godot;
using Yolk.Data;
using Yolk.Game;
using Yolk.Logic.Player;
using Yolk.Logic.World;
using Yolk.World;

public interface IPlayerProvider : INode, IProvide<IPlayerRepo> {
  public void Spawn(ITransform2D? transform);
  public void Despawn();
}

[Meta(typeof(IAutoNode))]
public partial class PlayerProvider : Node, IPlayerProvider {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private PackedScene PlayerScene { get; set; } = default!;
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private IWorldRepo WorldRepo => this.DependOn<IWorldRepo>();
  [Dependency] private ISaveChunk<GameData> GameChunk => this.DependOn<ISaveChunk<GameData>>();


  public PlayerRepo PlayerRepo { get; set; } = new(3, 1);
  private ISaveChunk<PlayerData> PlayerChunk { get; set; } = default!;
  private Player? Player { get; set; }

  IPlayerRepo IProvide<IPlayerRepo>.Value() => PlayerRepo;

  public void Setup() {

  }

  public void OnResolved() {
    GameChunk.OverwriteChunk(PlayerChunk);
    GameRepo.Starting += OnGameStarting;
    WorldRepo.Transitioned += OnWorldTransitioned;
    PlayerRepo.Respawned += OnPlayerRespawned;

    this.Provide();
  }

  private void OnGameReady() => throw new NotImplementedException();

  private void OnGameStarting() {
    PlayerRepo.SetHealth(3);
    PlayerRepo.SetMaxHealth(5);
    PlayerRepo.SetCharges(3);
    PlayerRepo.SetMaxCharges(3);
  }

  private void OnPlayerRespawned(ITransform2D? transform) {
    Despawn();
    Spawn(transform);
  }

  public void Spawn(ITransform2D? transform) {
    Player = PlayerScene.Instantiate<Player>();

    Player.GlobalPosition = transform is null
      ? new(0, 0)
      : new(transform.Position.X, transform.Position.Y);
    Player.Name = "Player";

    AddChild(Player);

    (Player as IPlayer).RegisterCheckpoint(Player.GlobalPosition.X, Player.GlobalPosition.Y);
  }

  public void Despawn() {
    if (Player is not null) {
      Player.QueueFree();
      Player.Name += "_REMOVING";
      Player = null;
    }
  }

  private void OnWorldTransitioned(ITransform2D? transform) {
    Despawn();
    Spawn(transform);
    GameRepo.BroadcastReady();
  }
}
