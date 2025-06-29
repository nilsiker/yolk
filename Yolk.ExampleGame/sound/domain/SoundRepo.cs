namespace Yolk;

using System;
using Godot;

public interface ISoundRepo {
  public event Action<ISound, Vector3>? Sounded;
  public void MakeSound(ISound sound, Vector3 atPosition);
}

public class SoundRepo : ISoundRepo {
  public event Action<ISound, Vector3>? Sounded;
  public void MakeSound(ISound sound, Vector3 atPosition) => Sounded?.Invoke(sound, atPosition);
}

