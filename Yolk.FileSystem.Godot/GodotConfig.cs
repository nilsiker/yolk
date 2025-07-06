namespace Yolk.FS;

using Godot;
using Godot.Collections;

public static class GodotConfig {
  private const string OPTIONS_PATH = "user://options.cfg";
  private const string INPUTMAP_PATH = "user://inputmap.cfg";
  private const string CONTROLS_SECTION = "controls";
  private const string DISPLAY_SECTION = "display";
  private const string GRAPHICS_SECTION = "graphics";
  private const string AUDIO_SECTION = "audio";

  public static void ImportInputMap() {
    var config = new ConfigFile();
    var error = config.Load(INPUTMAP_PATH);

    if (error == Error.Ok) {
      foreach (var key in config.GetSectionKeys(CONTROLS_SECTION)) {
        var events = (Array<InputEvent>)config.GetValue(CONTROLS_SECTION, key, InputMap.ActionGetEvents(key));
        InputMap.ActionEraseEvents(key);

        foreach (var @event in events) {
          InputMap.ActionAddEvent(key, @event);
        }
      }
    }
  }

  public static void ClearCustomInputMap() {
    var config = new ConfigFile();
    config.Clear();
    config.Save(INPUTMAP_PATH);
  }

  public static void WriteMappedAction(string action) {
    var events = InputMap.ActionGetEvents(action);

    var config = new ConfigFile();
    config.Load(INPUTMAP_PATH);

    config.SetValue(CONTROLS_SECTION, action, events);

    var error = config.Save(INPUTMAP_PATH);

    if (error != Error.Ok) {
      GD.PushWarning("failed to save inputs");
    }
  }

  public static void WriteDisplaySetting(string property, Variant value) => WriteSetting(DISPLAY_SECTION, property, value);

  public static void WriteGraphicsSetting(string property, Variant value) => WriteSetting(GRAPHICS_SECTION, property, value);

  public static void WriteAudioSetting(string property, Variant value) => WriteSetting(AUDIO_SECTION, property, value);

  private static void WriteSetting(string section, string property, Variant value) {
    var config = new ConfigFile();
    config.Load(OPTIONS_PATH);

    config.SetValue(section, property, value);

    var error = config.Save(OPTIONS_PATH);

    if (error != Error.Ok) {
      GD.PushWarning($"failed to save [{section}] {property} = {value}");
    }
  }
}
