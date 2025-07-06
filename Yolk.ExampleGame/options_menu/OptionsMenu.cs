namespace Yolk.UI;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.FS;
using Yolk.Game;
using Yolk.Options.Actions;

[Meta(typeof(IAutoNode))]
public partial class OptionsMenu : Control {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IOptionsRepo OptionsRepo => this.DependOn<IOptionsRepo>();
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IActionRepo ActionRepo => this.DependOn<IActionRepo>();

  [Node] private CheckButton FullscreenCheckButton { get; set; } = default!;
  [Node] private CheckButton VsyncCheckButton { get; set; } = default!;
  [Node] private CheckButton PixelationCheckButton { get; set; } = default!;
  [Node] private CheckButton DitheringCheckButton { get; set; } = default!;
  [Node] private HSlider MasterVolumeSlider { get; set; } = default!;
  [Node] private HSlider MusicVolumeSlider { get; set; } = default!;
  [Node] private HSlider SFXVolumeSlider { get; set; } = default!;
  [Node] private Button CloseButton { get; set; } = default!;
  [Node] private Button RestoreControlDefaultsButton { get; set; } = default!;

  private OptionsMenuLogic Logic { get; set; } = new();
  private OptionsMenuLogic.IBinding Binding { get; set; } = default!;

  public void OnResolved() {
    Binding = Logic.Bind();

    Binding.Handle((in OptionsMenuLogic.Output.UpdateVisibility output) => OnOutputUpdateVisibility(output.Visible));

    Logic.Set(GameRepo);
    Logic.Set(AppRepo);
    Logic.Set(OptionsRepo);

    // TODO move these to logic block!
    FullscreenCheckButton.Toggled += OnFullscreenCheckButtonToggled;
    VsyncCheckButton.Toggled += OnVsyncCheckButtonToggled;
    PixelationCheckButton.Toggled += OnPixelationCheckButtonToggled;
    DitheringCheckButton.Toggled += OnDitheringCheckButtonToggled;
    MasterVolumeSlider.ValueChanged += OnMasterVolumeSliderValueChanged;
    MusicVolumeSlider.ValueChanged += OnMusicVolumeSliderValueChanged;
    SFXVolumeSlider.ValueChanged += OnSFXVolumeSliderValueChanged;
    CloseButton.Pressed += OnCloseButtonPressed;

    OptionsRepo.UIVisible.Sync += OnOutputUpdateVisibility;

    OptionsRepo.Pixelation.Sync += OnOptionsPixelationSync;
    OptionsRepo.Dithering.Sync += OnOptionsDitheringSync;
    OptionsRepo.MasterVolume.Sync += OnOptionsMasterVolumeSync;
    OptionsRepo.MusicVolume.Sync += OnOptionsMusicVolumeSync;
    OptionsRepo.SFXVolume.Sync += OnOptionsSFXVolumeSync;

    RestoreControlDefaultsButton.Pressed += () => {
      GodotConfig.ClearCustomInputMap();
      InputMap.LoadFromProjectSettings();
      ActionRepo.BroadcastDefaultsRestored();
    };

    GameRepo.Quitted += () => OptionsRepo.SetUIVisible(false);
    GameRepo.Starting += () => OptionsRepo.SetUIVisible(false);

  }

  private void OnFullscreenCheckButtonToggled(bool enabled) => OptionsRepo.SetFullscreen(enabled);
  private void OnVsyncCheckButtonToggled(bool enabled) => OptionsRepo.SetVSync(enabled);
  private void OnPixelationCheckButtonToggled(bool on) {
    OptionsRepo.SetPixelation(on);
    DitheringCheckButton.Disabled = !on;
  }

  private void OnDitheringCheckButtonToggled(bool on) => OptionsRepo.SetDithering(on);
  private void OnSFXVolumeSliderValueChanged(double value) => OptionsRepo.SetSFXVolume((float)value);
  private void OnMusicVolumeSliderValueChanged(double value) => OptionsRepo.SetMusicVolume((float)value);
  private void OnMasterVolumeSliderValueChanged(double value) => OptionsRepo.SetMasterVolume((float)value);
  private void OnCloseButtonPressed() => OptionsRepo.SetUIVisible(false);

  private void OnOptionsPixelationSync(bool enabled) => PixelationCheckButton.SetPressedNoSignal(enabled);
  private void OnOptionsDitheringSync(bool enabled) => DitheringCheckButton.SetPressedNoSignal(enabled);
  private void OnOptionsMasterVolumeSync(float volume) => MasterVolumeSlider.SetValueNoSignal(volume);
  private void OnOptionsMusicVolumeSync(float volume) => MusicVolumeSlider.SetValueNoSignal(volume);
  private void OnOptionsSFXVolumeSync(float volume) => SFXVolumeSlider.SetValueNoSignal(volume);
  private void OnOutputUpdateVisibility(bool visible) => Visible = visible;
}
