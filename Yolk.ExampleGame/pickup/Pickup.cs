namespace Yolk.ExampleGame;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode))]
public partial class Pickup : Area2D {
  public override void _Notification(int what) => this.Notify(what);

  [Node] private AnimationPlayer AnimationPlayer { get; set; } = default!;

  public void OnResolved() {
    BodyEntered += OnBodyEntered;

    AnimationPlayer.AnimationFinished += OnAnimationFinished;
  }

  private void OnAnimationFinished(StringName animName) {
    if (animName == "picked_up") {
      QueueFree();
    }
  }

  private void OnBodyEntered(Node2D body) {
    if (body is IPlayer player) {
      player.Heal(1);
      AnimationPlayer.Play("picked_up");
    }
  }
}
