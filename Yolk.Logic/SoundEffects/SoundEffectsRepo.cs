namespace Yolk.Logic.SoundEffects;

public interface ISoundEffectsRepo {
  public event Action<string>? Played;
  public void Play(string soundPath);
}

public class SoundEffectsRepo : ISoundEffectsRepo {
  public event Action<string>? Played;

  public void Play(string soundPath)
    => Played?.Invoke(soundPath);
}
