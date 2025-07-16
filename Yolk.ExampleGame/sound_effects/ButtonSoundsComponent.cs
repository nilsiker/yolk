namespace Yolk.ExampleGame.SoundEffects;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.SoundEffects;


[Meta(typeof(IAutoNode))]
public partial class ButtonSoundsComponent : Node {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private ISoundEffectsRepo SoundEffects => this.DependOn<ISoundEffectsRepo>();

  [Export] private ButtonSounds? ButtonSounds { get; set; }

  private Button _button = default!;

  public void OnResolved() {
    _button = GetParentOrNull<Button>();
    if (_button == null) {
      GD.PrintErr($"ButtonSoundsComponent must be a child of a Button node. ({GetPath()})");
      QueueFree();
      return;
    }

    _button.ButtonUp += OnButtonUp;
    _button.ButtonDown += OnButtonDown;
    _button.FocusEntered += OnFocusEntered;
    _button.FocusExited += OnFocusExited;
    _button.Pressed += OnButtonPressed;
  }

  private void OnButtonUp() => PlaySound(ButtonSounds?.ButtonUpSound);
  private void OnButtonDown() => PlaySound(ButtonSounds?.ButtonDownSound);
  private void OnButtonPressed() => PlaySound(ButtonSounds?.PressedSound);
  private void OnFocusExited() => PlaySound(ButtonSounds?.FocusExitedSound);
  private void OnFocusEntered() => PlaySound(ButtonSounds?.FocusEnteredSound);

  private void PlaySound(AudioStream? sound) {
    if (sound is not null) {
      SoundEffects.Play(sound.ResourcePath);
    }
  }
}
