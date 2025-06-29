
namespace Yolk;

using Chickensoft.Collections;

public delegate void BlackoutCallback();

public interface IAppRepo {
  public event Action? QuitRequested;
  public event Action<BlackoutCallback>? BlackoutRequested;
  public IAutoProp<int> MouseReleases { get; }
  public void RequestQuit();
  public void CaptureMouse();
  public void ReleaseMouse();
  public void RequestBlackout(BlackoutCallback callback);
}
public class AppRepo : IAppRepo, IDisposable {
  public event Action? QuitRequested;
  public event Action<BlackoutCallback>? BlackoutRequested;
  public IAutoProp<int> MouseReleases => _mouseReleases;
  private readonly AutoProp<int> _mouseReleases = new(0);
  public void RequestQuit() => QuitRequested?.Invoke();
  public void CaptureMouse() => _mouseReleases.OnNext(Math.Max(0, _mouseReleases.Value - 1));
  public void ReleaseMouse() => _mouseReleases.OnNext(_mouseReleases.Value + 1);
  public void RequestBlackout(BlackoutCallback callback) => BlackoutRequested?.Invoke(callback);

  public void Dispose() {
    _mouseReleases.OnCompleted();
    _mouseReleases.Dispose();

    GC.SuppressFinalize(this);
  }
}
