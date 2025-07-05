namespace Yolk.Options;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode))]
public partial class AudioController : Node {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IOptionsRepo OptionsRepo => this.DependOn<IOptionsRepo>();

  public void OnResolved() {
    OptionsRepo.MasterVolume.Sync += OnOptionsMasterVolumeSync;
    OptionsRepo.MusicVolume.Sync += OnOptionsMusicVolumeSync;
    OptionsRepo.SFXVolume.Sync += OnOptionsSFXVolumeSync;
  }

  private void OnOptionsMasterVolumeSync(float volume) => AudioServer.Singleton.SetBusVolumeLinear(0, volume);
  private void OnOptionsMusicVolumeSync(float volume) => AudioServer.Singleton.SetBusVolumeLinear(1, volume);
  private void OnOptionsSFXVolumeSync(float volume) => AudioServer.Singleton.SetBusVolumeLinear(2, volume);

  public override void _ExitTree() {
    OptionsRepo.MasterVolume.Sync -= OnOptionsMasterVolumeSync;
    OptionsRepo.MusicVolume.Sync -= OnOptionsMusicVolumeSync;
    OptionsRepo.SFXVolume.Sync -= OnOptionsSFXVolumeSync;
  }
}

