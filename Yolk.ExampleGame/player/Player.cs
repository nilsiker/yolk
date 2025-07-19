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

  [Node] private Sprite2D Sprite { get; set; } = default!;
  [Node] private AnimationPlayer Anim { get; set; } = default!;
  [Node] private Area2D Hurtbox { get; set; } = default!;

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

    Hurtbox.BodyEntered += OnHurtboxBodyEntered;
  }

  private void OnHurtboxBodyEntered(Node2D body) => (this as IKillable).Kill();
  private void OnOutputSetEnabled(bool enabled) => SetCollisionLayerValue(1, enabled);

  private void OnOutputTeleport(ITransform2D entrypoint) {
    GD.Print($"Teleporting player to {entrypoint.Position}");
    GlobalPosition = new Vector2(entrypoint.Position.X, entrypoint.Position.Y);
  }

  private float _gravityForce = 9.82f * 3.0f;
  private float _gravity;
  private float _horizontalForce;
  private float _coyoteTime;
  private float _wallCoyoteTime;

  public override void _PhysicsProcess(double delta) {
    var inputVector = Inputs.GetMoveVector();

    var pushingAgainstWall = IsOnWall() && !GetWallNormal().IsZeroApprox() && Mathf.Abs(inputVector.X + GetWallNormal().X) < Mathf.Epsilon;

    if (inputVector.X != 0) {
      _horizontalForce = 0;
      Sprite.FlipH = inputVector.X < 0;
      Anim.Play("walk");
    }
    else {
      Anim.Play("idle");
    }

    if (!IsOnFloor()) {
      Anim.Play("jump");
    }

    if (pushingAgainstWall) {
      Anim.Play("hang");
    }

    _coyoteTime = IsOnFloor()
      ? 0.05f
      : Mathf.MoveToward(_coyoteTime, 0, (float)delta);

    _wallCoyoteTime = IsOnWall()
      ? 0.1f
      : Mathf.MoveToward(_wallCoyoteTime, 0, (float)delta);

    Velocity = Velocity with {
      X = (inputVector.X * 75) + _horizontalForce,
      Y = _gravity * 10
    };

    _horizontalForce = IsOnFloor() ? 0 : Mathf.MoveToward(_horizontalForce, 0, (float)delta * 10);
    MoveAndSlide();

    _gravity = (IsOnFloor(), IsOnCeiling(), IsOnWall() && pushingAgainstWall) switch {
      (false, false, false) => _gravity + (_gravityForce * (float)delta),
      (false, true, false) => _gravityForce * (float)delta,
      (false, false, true) => _gravityForce / 15.0f,
      _ => 0
    };
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(Inputs.Jump)) {
      if (_coyoteTime > 0) {
        _gravity = -10;
      }
      else if (_wallCoyoteTime > 0) {
        _gravity = -8f;
        _horizontalForce = GetWallNormal().X * 50;
      }
    }
  }
  public override void _ExitTree() => Binding.Dispose();
}
