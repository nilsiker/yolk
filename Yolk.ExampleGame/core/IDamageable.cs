namespace Yolk;

using System;

public interface IDamageable {
  public event Action<float>? Damaged;
  public void Damage(float damage);
}
