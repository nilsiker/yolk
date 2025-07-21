
namespace Yolk.Logic.Player;

using Chickensoft.Collections;
using Yolk.Logic.World;

public interface IPlayerRepo : IDisposable {
  public event Action? OutOfHearts;
  public event Action? Damaged;
  public event Action? Healed;
  public event Action<ITransform2D>? Respawned;
  public IAutoProp<int> Health { get; }
  public IAutoProp<int> MaxHealth { get; }
  public IAutoProp<int> Charges { get; }
  public IAutoProp<int> MaxCharges { get; }


  public void SetHealth(int hearts);
  public void SetMaxHealth(int maxHearts);
  public void SetCharges(int charges);
  public void SetMaxCharges(int maxCharges);
  public void Damage(int amount = 1);
  public void Heal(int amount = 1);
  public void AddCharge(int amount = 1);
  public void RemoveCharge(int amount = 1);
  public void Respawn(ITransform2D transform);
}

public class PlayerRepo(int initialHearts, int initialCharges) : IPlayerRepo {
  private readonly AutoProp<int> _hearts = new(initialHearts);
  private readonly AutoProp<int> _maxHearts = new(3);
  private readonly AutoProp<int> _charges = new(initialCharges);
  private readonly AutoProp<int> _maxCharges = new(1);

  public event Action? OutOfHearts;
  public event Action? Damaged;
  public event Action? Healed;
  public event Action<ITransform2D>? Respawned;

  public IAutoProp<int> Health => _hearts;
  public IAutoProp<int> MaxHealth => _maxHearts;
  public IAutoProp<int> Charges => _charges;
  public IAutoProp<int> MaxCharges => _maxCharges;

  public void SetHealth(int hearts) {
    _hearts.OnNext(Math.Max(hearts, 0));
    if (_hearts.Value <= 0) {
      OutOfHearts?.Invoke();
    }
  }

  public void SetMaxHealth(int maxHearts) => _maxHearts.OnNext(Math.Max(maxHearts, 1));

  public void SetCharges(int charges) => _charges.OnNext(Math.Max(charges, 0));

  public void SetMaxCharges(int maxCharges) => _maxCharges.OnNext(Math.Max(maxCharges, 0));

  public void Damage(int amount = 1) {
    var newHearts = Math.Max(_hearts.Value - amount, 0);
    _hearts.OnNext(newHearts);
    if (newHearts == 0) {
      OutOfHearts?.Invoke();
    }
    Damaged?.Invoke();
  }

  public void Heal(int amount = 1) {
    var newHearts = Math.Min(_hearts.Value + amount, _maxHearts.Value);
    _hearts.OnNext(newHearts);
    Healed?.Invoke();
  }

  public void AddCharge(int amount = 1) {
    var newCharges = Math.Min(_charges.Value + amount, _maxCharges.Value);
    _charges.OnNext(newCharges);
  }

  public void RemoveCharge(int amount = 1) {
    var newCharges = Math.Max(_charges.Value - amount, 0);
    _charges.OnNext(newCharges);
  }
  public void Respawn(ITransform2D transform) => Respawned?.Invoke(transform);

  public void Dispose() {
    _hearts.OnCompleted();
    _maxHearts.OnCompleted();
    _charges.OnCompleted();
    _maxCharges.OnCompleted();

    _hearts.Dispose();
    _maxHearts.Dispose();
    _charges.Dispose();
    _maxCharges.Dispose();

    GC.SuppressFinalize(this);
  }

}
