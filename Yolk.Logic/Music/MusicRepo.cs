namespace Yolk.Logic.Music;

public interface IMusicRepo {
  public event Action<string, float, float>? Started;
  public event Action? Stopped;
  public void Start(string musicName, float crossfade = 0.0f, float delay = 0.0f);
  public void Stop();
}

public class MusicRepo : IMusicRepo {
  public event Action<string, float, float>? Started;
  public event Action? Stopped;
  public void Start(string musicName, float crossfade = 0.0f, float delay = 0.0f)
    => Started?.Invoke(musicName, crossfade, delay);
  public void Stop() => Stopped?.Invoke();
}
