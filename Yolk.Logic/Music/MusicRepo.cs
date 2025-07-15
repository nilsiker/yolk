namespace Yolk.Logic.Music;

public interface IMusicRepo {
  public event Action<string>? Started;
  public event Action? Stopped;
  public void Start(string musicName);
  public void Stop();
}

public class MusicRepo : IMusicRepo {
  public event Action<string>? Started;
  public event Action? Stopped;
  public void Start(string musicName) => Started?.Invoke(musicName);
  public void Stop() => Stopped?.Invoke();
}
