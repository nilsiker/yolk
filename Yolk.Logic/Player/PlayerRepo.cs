
namespace Yolk.Logic.Player;

using Chickensoft.Collections;

public interface IPlayerRepo : IDisposable {
  public event Action? OutOfHearts;
  public event Action? Damaged;
  public event Action? Healed;
  public IAutoProp<int> Hearts { get; }
  public IAutoProp<int> MaxHearts { get; }
  public IAutoProp<int> Charges { get; }
  public IAutoProp<int> MaxCharges { get; }

  public void Damage(int amount = 1);
  public void Heal(int amount = 1);
  public void AddCharge(int amount = 1);
  public void RemoveCharge(int amount = 1);
}

public class PlayerRepo(int initialHearts, int initialCharges) : IPlayerRepo {
  private readonly AutoProp<int> _hearts = new(initialHearts);
  private readonly AutoProp<int> _maxHearts = new(3);
  private readonly AutoProp<int> _charges = new(initialCharges);
  private readonly AutoProp<int> _maxCharges = new(1);

  public event Action? OutOfHearts;
  public event Action? Damaged;
  public event Action? Healed;

  public IAutoProp<int> Hearts => _hearts;
  public IAutoProp<int> MaxHearts => _maxHearts;
  public IAutoProp<int> Charges => _charges;
  public IAutoProp<int> MaxCharges => _maxCharges;

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
