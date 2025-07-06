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
  public event Action<string>? LoadRequested;
  public event Action? Saved;
  public event Action? AutosaveRequested;
  public event Action? AutoloadRequested;
  public event Action? QuicksaveRequested;
  public event Action? QuickloadRequested;

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
  public void BroadcastSaved();
  public void Autosave();
  public void Autoload();
  public void Quicksave();
  public void Quickload();
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
  public event Action? AutosaveRequested;
  public event Action? AutoloadRequested;
  public event Action? QuicksaveRequested;
  public event Action? QuickloadRequested;


  private readonly AutoProp<EPauseMode> _pauseMode = new(EPauseMode.NotPaused);
  public IAutoProp<EPauseMode> PauseMode => _pauseMode;


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
  public void BroadcastSaved() => Saved?.Invoke();
  public void Autosave() => AutosaveRequested?.Invoke();
  public void Autoload() => AutoloadRequested?.Invoke();
  public void Quicksave() => QuicksaveRequested?.Invoke();
  public void Quickload() => QuickloadRequested?.Invoke();

  public void Dispose() {
    _pauseMode.OnCompleted();
    _pauseMode.Dispose();

    GC.SuppressFinalize(this);
  }
}
