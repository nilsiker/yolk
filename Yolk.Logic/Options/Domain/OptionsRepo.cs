namespace Yolk;

using System;
using Chickensoft.Collections;

public interface IOptionsRepo : IDisposable {
  public event Action? OptionChanged;
  public IAutoProp<bool> UIVisible { get; }
  public IAutoProp<bool> Fullscreen { get; }
  public IAutoProp<bool> Vsync { get; }
  public IAutoProp<bool> Pixelation { get; }
  public IAutoProp<bool> Dithering { get; }
  public IAutoProp<float> MasterVolume { get; }
  public IAutoProp<float> MusicVolume { get; }
  public IAutoProp<float> SFXVolume { get; }

  public void SetUIVisible(bool visible);
  // Display
  public void SetFullscreen(bool fullscreen);
  public void SetVSync(bool vsync);
  // Graphics
  public void SetPixelation(bool enabled);
  public void SetDithering(bool enabled);
  // Audio
  public void SetMasterVolume(float volumeDb);
  public void SetMusicVolume(float volumeDb);
  public void SetSFXVolume(float volumeDb);
}

public class OptionsRepo(
  bool fullscreen = true,
  bool vsync = false,
  bool pixelation = true,
  bool dithering = true,
  float masterVolume = 0,
  float musicVolume = 0,
  float sfxVolume = 0
) : IOptionsRepo {
  private readonly AutoProp<bool> _uiVisible = new(false);
  private readonly AutoProp<bool> _fullscreen = new(fullscreen);
  private readonly AutoProp<bool> _vsync = new(vsync);
  private readonly AutoProp<bool> _dithering = new(dithering);
  private readonly AutoProp<bool> _pixelation = new(pixelation);
  private readonly AutoProp<float> _masterVolume = new(masterVolume);
  private readonly AutoProp<float> _musicVolume = new(musicVolume);
  private readonly AutoProp<float> _sfxVolume = new(sfxVolume);

  public event Action? OptionChanged;
  public IAutoProp<bool> Fullscreen => _fullscreen;
  public IAutoProp<bool> Vsync => _vsync;
  public IAutoProp<bool> UIVisible => _uiVisible;
  public IAutoProp<bool> Pixelation => _pixelation;
  public IAutoProp<bool> Dithering => _dithering;
  public IAutoProp<float> MasterVolume => _masterVolume;
  public IAutoProp<float> MusicVolume => _musicVolume;
  public IAutoProp<float> SFXVolume => _sfxVolume;

  public void SetUIVisible(bool visible) => _uiVisible.OnNext(visible);

  public void SetFullscreen(bool fullscreen) {
    _fullscreen.OnNext(fullscreen);
    OptionChanged?.Invoke();
  }

  public void SetVSync(bool vsync) {
    _vsync.OnNext(vsync);
    OptionChanged?.Invoke();
  }

  public void SetPixelation(bool enabled) {
    _pixelation.OnNext(enabled);
    OptionChanged?.Invoke();
  }

  public void SetDithering(bool enabled) {
    _dithering.OnNext(enabled);
    OptionChanged?.Invoke();
  }

  public void SetMasterVolume(float volumeDb) {
    _masterVolume.OnNext(volumeDb);
    OptionChanged?.Invoke();
  }

  public void SetMusicVolume(float volumeDb) {
    _musicVolume.OnNext(volumeDb);
    OptionChanged?.Invoke();
  }

  public void SetSFXVolume(float volumeDb) {
    _sfxVolume.OnNext(volumeDb);
    OptionChanged?.Invoke();
  }

  public void Dispose() {
    _uiVisible.OnCompleted();
    _uiVisible.Dispose();

    _pixelation.OnCompleted();
    _pixelation.Dispose();

    _dithering.OnCompleted();
    _dithering.Dispose();

    _masterVolume.OnCompleted();
    _masterVolume.Dispose();

    _musicVolume.OnCompleted();
    _musicVolume.Dispose();

    _sfxVolume.OnCompleted();
    _sfxVolume.Dispose();

    GC.SuppressFinalize(this);
  }
}
