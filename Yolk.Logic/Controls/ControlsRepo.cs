
namespace Yolk.Logic.Controls;

public class ControlsRepo {
  public event Action<string, EActionType>? Mapping;
  public event Action<string, string>? ActionMapped;
  public event Action? Cancelled;

  public void RequestMapping(string action, EActionType actionType) => Mapping?.Invoke(action, actionType);
  public void BroadcastActionMapped(string action, string key) => ActionMapped?.Invoke(action, key);
  public void Cancel() => Cancelled?.Invoke();
}
