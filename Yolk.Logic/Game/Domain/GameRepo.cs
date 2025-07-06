namespace Yolk.Game;

using System;
using Chickensoft.Collections;

public interface IGameRepo {
  public event Action? Start;
  public event Action? Starting;
  public event Action? Ready;
  public event Action? QuitRequested;
  public event Action? GameOverRequested;
  public event Action? GameOver;
  public event Action? Quitted;
  public event Action<string>? SaveRequested;
  public event Action<string>? Saved;
  public event Action<string>? LoadRequested;

  public IAutoProp<string> LastSaveName { get; }
  public IAutoProp<EPauseMode> PauseMode { get; }

  public void RequestStart();
  public void BroadcastStarting();
  public void BroadcastReady();
  public void RequestQuit();
  public void BroadcastQuitted();
  public void RequestGameOver();
  public void BroadcastGameOver();
  public void Pause(bool pausedByPlayer = false);
  public void Resume();
  public void Save(string saveName);
  public void Load(string saveName);
  public void SetLastSaveName(string saveName);
}

public class GameRepo : IGameRepo, IDisposable {
  public event Action? Start;
  public event Action? Starting;
  public event Action? Ready;

  public event Action? QuitRequested;
  public event Action? Quitted;
  public event Action? GameOverRequested;
  public event Action<string>? SaveRequested;
  public event Action<string>? Saved;
  public event Action<string>? LoadRequested;
  public event Action? GameOver;


  private readonly AutoProp<EPauseMode> _pauseMode = new(EPauseMode.NotPaused);
  private readonly AutoProp<string> _lastSaveName = new("Autosave");
  public IAutoProp<EPauseMode> PauseMode => _pauseMode;
  public IAutoProp<string> LastSaveName => _lastSaveName;

  public void RequestStart() => Start?.Invoke();
  public void BroadcastStarting() => Starting?.Invoke();
  public void BroadcastReady() => Ready?.Invoke();
  public void RequestQuit() => QuitRequested?.Invoke();
  public void BroadcastQuitted() => Quitted?.Invoke();
  public void RequestGameOver() => GameOverRequested?.Invoke();
  public void BroadcastGameOver() => GameOver?.Invoke();

  public void Pause(bool pausedByPlayer = false) => _pauseMode.OnNext(pausedByPlayer ? EPauseMode.PausedByPlayer : EPauseMode.Paused);
  public void Resume() => _pauseMode.OnNext(EPauseMode.NotPaused);
  public void Save(string saveName) => SaveRequested?.Invoke(saveName);
  public void Load(string saveName) => LoadRequested?.Invoke(saveName);
  public void SetLastSaveName(string? saveName) {
    var newLastSaveName = (_lastSaveName.Value, saveName) switch {
      (_, "") => throw new ArgumentException("Save name cannot be empty"),
      ("", _) => throw new ArgumentException("Last save name cannot be empty"),
      (not null, null) => $"{_lastSaveName}_Autosave",
      (_, not null) => saveName,
      _ => "Autosave",
    };

    _lastSaveName.OnNext(newLastSaveName);
  }

  public void Dispose() {
    _pauseMode.OnCompleted();
    _pauseMode.Dispose();
    _lastSaveName.OnCompleted();
    _lastSaveName.Dispose();

    GC.SuppressFinalize(this);
  }
}
