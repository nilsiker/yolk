namespace Yolk.Logic.SoundEffects;

public interface ISoundEffectsRepo {
  public event Action<string>? SoundPlayed;
  public event Action<string, (float x, float y, float? z)>? SoundPlayedAt;
  public void Play(string soundName);
  public void PlayAt(string soundName, (float x, float y, float? z) position);
}

public class SoundEffectsRepo : ISoundEffectsRepo {
  public event Action<string>? SoundPlayed;
  public event Action<string, (float x, float y, float? z)>? SoundPlayedAt;

  public void Play(string soundName) => SoundPlayed?.Invoke(soundName);
  public void PlayAt(string soundName, (float x, float y, float? z) position)
    => SoundPlayedAt?.Invoke(soundName, position);
}
