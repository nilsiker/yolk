namespace Yolk.Logic.SoundEffects2D;

public interface ISoundEffects2DRepo {
  public event Action<string, (float x, float y)>? Played;
  public void Play(string soundPath, (float x, float y) position);
}

public class SoundEffects2DRepo : ISoundEffects2DRepo {
  public event Action<string, (float x, float y)>? Played;
  public void Play(string soundPath, (float x, float y) position)
    => Played?.Invoke(soundPath, position);
}
