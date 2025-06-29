namespace Yolk;

using System;
using Chickensoft.Collections;
using Godot;

public partial class HealthComponent : Node, IDamageable {
  private readonly AutoProp<float> _health = new(3f);
  public IAutoProp<float> Health => _health;

  public event Action<float>? Damaged;
  public event Action? Died;

  public void Damage(float damage) {
    var newHealth = _health.Value - damage;

    _health.OnNext(Mathf.Max(0, newHealth));

    if (newHealth <= 0) {
      Died?.Invoke();
    }
    else {
      Damaged?.Invoke(damage);
    }
  }

  public override void _ExitTree() {
    _health.OnCompleted();
    _health.Dispose();
  }
}
