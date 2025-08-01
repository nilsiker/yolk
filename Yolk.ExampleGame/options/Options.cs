namespace Yolk.Options;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

public interface IOptions {
  public IOptionsRepo OptionsRepo { get; }
}

[Meta(typeof(IAutoNode))]
public partial class Options : Node, IOptions {
  public override void _Notification(int what) => this.Notify(what);

  public IOptionsRepo OptionsRepo { get; private set; } = GetOptionsRepo();

  public void OnResolved() => OptionsRepo.OptionChanged += SaveOptionsConfigFile;

  private static OptionsRepo GetOptionsRepo() {
    var config = new ConfigFile();

    var error = config.Load("user://options.cfg");

    return error == Error.Ok
      ? new(
        (config.GetValue("display", "resolution").AsVector2I().X, config.GetValue("display", "resolution").AsVector2I().Y),
        config.GetValue("display", "fullscreen").AsBool(),
        config.GetValue("display", "vsync").AsBool(),
        config.GetValue("graphics", "pixelation").AsBool(),
        config.GetValue("graphics", "dithering").AsBool(),
        (float)config.GetValue("audio", "master_volume").AsDouble(),
        (float)config.GetValue("audio", "music_volume").AsDouble(),
        (float)config.GetValue("audio", "sfx_volume").AsDouble()
      )
      : new((DisplayServer.Singleton.ScreenGetSize().X, DisplayServer.Singleton.ScreenGetSize().Y));
  }

  private void SaveOptionsConfigFile() {
    var config = new ConfigFile();

    config.SetValue("display", "resolution", new Vector2I(OptionsRepo.Resolution.Value.Item1, OptionsRepo.Resolution.Value.Item2));
    config.SetValue("display", "fullscreen", OptionsRepo.Fullscreen.Value);
    config.SetValue("display", "vsync", OptionsRepo.Vsync.Value);
    config.SetValue("graphics", "pixelation", OptionsRepo.Pixelation.Value);
    config.SetValue("graphics", "dithering", OptionsRepo.Dithering.Value);
    config.SetValue("audio", "master_volume", OptionsRepo.MasterVolume.Value);
    config.SetValue("audio", "music_volume", OptionsRepo.MusicVolume.Value);
    config.SetValue("audio", "sfx_volume", OptionsRepo.SFXVolume.Value);

    var error = config.Save("user://options.cfg");
    if (error != Error.Ok) {
      GD.PushError(error);
    }
  }
}
