namespace Yolk.World;

using Yolk.Logic.World;


public interface IWorldRepo {
  public event Action<string>? Transitioning;
  public event Action<Entrypoint?>? Transitioned;

  public void Transition(string toLevelName);
  public void BroadcastTransitioned(Entrypoint? entrypoint);
}

public class WorldRepo : IWorldRepo {
  public event Action<string>? Transitioning;
  public event Action<Entrypoint?>? Transitioned;

  public void Transition(string toLevelName) => Transitioning?.Invoke(toLevelName);
  public void BroadcastTransitioned(Entrypoint? entrypoint) => Transitioned?.Invoke(entrypoint);

}
