namespace Yolk;

using System;
using Godot;

[GlobalClass, Tool]
public partial class SoundResource : Resource, ISound {
  [Export] public AudioStream? Stream { get; set; }
  [Export] public float Distance { get; set; }

  string ISound.StreamPath => Stream?.ResourcePath ?? throw new MissingFieldException();
}
