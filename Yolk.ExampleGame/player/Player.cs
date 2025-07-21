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
using Yolk.Logic.SoundEffects;
using Yolk.World;

public interface IPlayer : ICharacterBody2D, IDamageable {
  public void RegisterCheckpoint(float x, float y);
  public void Heal(int amount = 1);
}

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class Player : CharacterBody2D, IPlayer {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private IWorldRepo WorldRepo => this.DependOn<IWorldRepo>();
  [Dependency] private IPlayerRepo PlayerRepo => this.DependOn<IPlayerRepo>();
  [Dependency] private ISoundEffectsRepo SoundEffectRepo => this.DependOn<ISoundEffectsRepo>();
  [Dependency] private ISaveChunk<GameData> GameChunk => this.DependOn<ISaveChunk<GameData>>();

  [Node] private Sprite2D Sprite { get; set; } = default!;
  [Node] private AnimationPlayer Anim { get; set; } = default!;
  [Node] private Area2D Hurtbox { get; set; } = default!;
  [Node] private CpuParticles2D JumpParticles { get; set; } = default!;

  void IPlayer.RegisterCheckpoint(float x, float y) => Logic.Input(new PlayerLogic.Input.RegisterCheckpoint(x, y));
  void IPlayer.Heal(int amount) => PlayerRepo.Heal(amount); // TODO go through logic later
  void IDamageable.TakeDamage(int amount) {
    if (!_invincible) {
      Logic.Input(new PlayerLogic.Input.TakeDamage(amount));
    }
  }

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
      .Handle((in PlayerLogic.Output.Teleport output) => OnOutputTeleport(output.X, output.Y))
      .Handle((in PlayerLogic.Output.Animate output) => OnOutputAnimate(output.Animation))
      .Handle((in PlayerLogic.Output.SetEnabled output) => OnOutputSetEnabled(output.Enabled))
      .Handle((in PlayerLogic.Output.MoveAndSlide output) => OnOutputMoveAndSlide(output.X, output.Y))
      .Handle((in PlayerLogic.Output.FaceRight _) => Sprite.FlipH = false)
      .Handle((in PlayerLogic.Output.FaceLeft _) => Sprite.FlipH = true)
      .Handle((in PlayerLogic.Output.OnJump output) => OnOutputOnJump())
      .Handle((in PlayerLogic.Output.GrantInvincibility output) => OnOutputGrantInvincibility(output.Duration));

    Logic.Set(AppRepo);
    Logic.Set(WorldRepo);
    Logic.Set(GameRepo);
    Logic.Set(PlayerRepo);
    Logic.Set(new PlayerLogic.Data {
      VelocityX = 0,
      VelocityY = 0
    });
    Logic.Start();

    Hurtbox.BodyEntered += OnHurtboxBodyEntered;
    Anim.AnimationFinished += OnAnimationFinished;
  }

  private bool _invincible;
  private void OnOutputGrantInvincibility(float duration) {
    _invincible = true;
    var tween = Sprite.CreateTween();

    var times = (int)duration * 3;

    tween
      .SetTrans(Tween.TransitionType.Sine)
      .SetEase(Tween.EaseType.InOut)
      .SetLoops(times);

    tween.TweenProperty(Sprite, "modulate", new Color(1, 1, 1, 0.5f), duration / times / 2);
    tween.TweenProperty(Sprite, "modulate", new Color(1, 1, 1, 1.0f), duration / times / 2);

    tween.TweenCallback(Callable.From(() => _invincible = false));
  }
  private void OnOutputOnJump() => JumpParticles.Restart();
  private void OnHurtboxBodyEntered(Node2D body) => (this as IDamageable).TakeDamage(1);
  private void OnAnimationFinished(StringName animName) => Logic.Input(new PlayerLogic.Input.AnimationFinished());

  private void OnOutputMoveAndSlide(float x, float y) {
    Velocity = new Vector2(x, y);
    MoveAndSlide();
  }


  private void OnOutputAnimate(string animation) => Anim.Play(animation);
  private void OnOutputSetEnabled(bool enabled) => Callable.From(() => SetCollisionLayerValue(1, enabled)).CallDeferred();
  private void OnOutputTeleport(float x, float y) => GlobalPosition = new Vector2(x, y);

  public override void _PhysicsProcess(double delta) {
    var inputVector = Inputs.GetMoveVector();
    Logic.Input(new PlayerLogic.Input.OnGrounded(IsOnFloor()));
    Logic.Input(new PlayerLogic.Input.Move(inputVector.X, inputVector.Y));


    if (IsOnCeiling()) {
      Logic.Input(new PlayerLogic.Input.HitCeiling());
    }

    if (IsOnWall()) {
      Logic.Input(new PlayerLogic.Input.ClingToWall());
    }

    Logic.Input(new PlayerLogic.Input.PhysicsTick((float)delta));
  }

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(Inputs.Jump)) {
      Logic.Input(new PlayerLogic.Input.Jump());
    }
    else if (@event.IsActionReleased(Inputs.Jump)) {
      Logic.Input(new PlayerLogic.Input.StopJump());
    }
  }
  public override void _ExitTree() => Binding.Dispose();
}
