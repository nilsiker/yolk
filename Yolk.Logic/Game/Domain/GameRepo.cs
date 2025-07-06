namespace Yolk.Game;

using System;
using Chickensoft.Collections;
using Yolk.Data;


public interface IGameRepo {
  public event Action? Start;
  public event Action? Starting;
  public event Action? Ready;
  public event Action? QuitRequested;
  public event Action? GameOverRequested;
  public event Action? GameOver;
  public event Action? Quitted;
  public event Action<string>? SaveRequested;
  public event Action<string>? LoadRequested;
  public event Action? Saved;

  public IAutoProp<string?> LastSaveName { get; }
  public IAutoProp<ESaveType> LastSaveType { get; }
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
  public void Save(string saveName, ESaveType saveType);
  public void Load(string saveName, ESaveType saveType = ESaveType.Manual);
  public void BroadcastSaved();
}

public class GameRepo : IGameRepo, IDisposable {
  public event Action? Start;
  public event Action? Starting;
  public event Action? Ready;

  public event Action? QuitRequested;
  public event Action? Quitted;
  public event Action? GameOverRequested;
  public event Action<string>? SaveRequested;
  public event Action<string>? LoadRequested;
  public event Action? GameOver;
  public event Action? Saved;

  private readonly AutoProp<EPauseMode> _pauseMode = new(EPauseMode.NotPaused);
  private readonly AutoProp<string?> _lastSaveName = new(default);
  private readonly AutoProp<ESaveType> _lastSaveType = new(ESaveType.Autosave);
  public IAutoProp<EPauseMode> PauseMode => _pauseMode;
  public IAutoProp<string?> LastSaveName => _lastSaveName;
  public IAutoProp<ESaveType> LastSaveType => _lastSaveType;


  public void RequestStart() => Start?.Invoke();
  public void BroadcastStarting() => Starting?.Invoke();
  public void BroadcastReady() => Ready?.Invoke();
  public void RequestQuit() => QuitRequested?.Invoke();
  public void BroadcastQuitted() => Quitted?.Invoke();
  public void RequestGameOver() => GameOverRequested?.Invoke();
  public void BroadcastGameOver() => GameOver?.Invoke();

  public void Pause(bool pausedByPlayer = false) => _pauseMode.OnNext(pausedByPlayer ? EPauseMode.PausedByPlayer : EPauseMode.Paused);
  public void Resume() => _pauseMode.OnNext(EPauseMode.NotPaused);

  public void Save(string saveName, ESaveType saveType) {
    _lastSaveName.OnNext(saveName);
    _lastSaveType.OnNext(saveType);
    SaveRequested?.Invoke(saveName);
  }

  public void Load(string saveName, ESaveType saveType = ESaveType.Manual) {
    _lastSaveName.OnNext(saveName);
    _lastSaveType.OnNext(saveType);
    LoadRequested?.Invoke(saveName);
  }

  public void BroadcastSaved() => Saved?.Invoke();

  public void Dispose() {
    _pauseMode.OnCompleted();
    _pauseMode.Dispose();
    _lastSaveName.OnCompleted();
    _lastSaveName.Dispose();

    GC.SuppressFinalize(this);
  }
}
