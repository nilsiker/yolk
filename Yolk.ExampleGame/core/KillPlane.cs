namespace Yolk.Core;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Game;


[Meta(typeof(IAutoNode))]
public partial class KillPlane : Area2D {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  public void OnResolved() => BodyEntered += (body) => {
    if (body is IDamageable damageable) {
      damageable.TakeDamage(1);
    }
  };
}
