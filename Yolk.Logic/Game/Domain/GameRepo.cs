namespace Yolk.Game;

using System;
using Chickensoft.Collections;

public interface IGameRepo {
  public event Action<int>? StartRequested;
  public event Action? Started;
  public event Action? QuitRequested;
  public event Action? GameOverRequested;
  public event Action? GameOver;
  public event Action? Quitted;
  public event Action<int>? SaveRequested;
  public event Action<int>? LoadRequested;

  public IAutoProp<int> Slot { get; }
  public IAutoProp<EPauseMode> PauseMode { get; }

  public void RequestStart(int slot);
  public void BroadcastStarted();
  public void RequestQuit();
  public void BroadcastQuitted();
  public void RequestGameOver();
  public void BroadcastGameOver();
  public void Pause(bool pausedByPlayer = false);
  public void Resume();
  public void Save(int? slot);
  public void Load(int? slot);
  public void SetSlot(int slot);
}

public class GameRepo : IGameRepo, IDisposable {
  public event Action<int>? StartRequested;
  public event Action? Started;

  public event Action? QuitRequested;
  public event Action? Quitted;
  public event Action? GameOverRequested;
  public event Action<int>? SaveRequested;
  public event Action<int>? LoadRequested;
  public event Action? GameOver;

  private readonly AutoProp<EPauseMode> _pauseMode = new(EPauseMode.NotPaused);
  private readonly AutoProp<int> _slot = new(0);
  public IAutoProp<EPauseMode> PauseMode => _pauseMode;
  public IAutoProp<int> Slot => _slot;

  public void RequestStart(int slot) => StartRequested?.Invoke(slot);
  public void BroadcastStarted() => Started?.Invoke();
  public void RequestQuit() => QuitRequested?.Invoke();
  public void BroadcastQuitted() => Quitted?.Invoke();
  public void RequestGameOver() => GameOverRequested?.Invoke();
  public void BroadcastGameOver() => GameOver?.Invoke();

  public void Pause(bool pausedByPlayer = false) => _pauseMode.OnNext(pausedByPlayer ? EPauseMode.PausedByPlayer : EPauseMode.Paused);
  public void Resume() => _pauseMode.OnNext(EPauseMode.NotPaused);
  public void Save(int? slot) => SaveRequested?.Invoke(slot ?? _slot.Value);
  public void Load(int? slot) => LoadRequested?.Invoke(slot ?? _slot.Value);
  public void SetSlot(int slot) => _slot.OnNext(slot);

  public void Dispose() {
    _pauseMode.OnCompleted();
    _pauseMode.Dispose();
    _slot.OnCompleted();
    _slot.Dispose();

    GC.SuppressFinalize(this);
  }
}
