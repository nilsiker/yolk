namespace Yolk.ExampleGame.SoundEffects;

using Godot;

[GlobalClass, Tool]
public partial class ButtonSounds : Resource {
  [Export] public AudioStream? ButtonUpSound { get; set; }
  [Export] public AudioStream? ButtonDownSound { get; set; }
  [Export] public AudioStream? PressedSound { get; set; }
  [Export] public AudioStream? FocusExitedSound { get; set; }
  [Export] public AudioStream? FocusEnteredSound { get; set; }
}
