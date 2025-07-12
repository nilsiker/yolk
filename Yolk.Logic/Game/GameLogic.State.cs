namespace Yolk.Game;

using System;
using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State : StateLogic<State>, IGet<Input.Load>, IGet<Input.OnSaved>, IGet<Input.DeleteSave> {
    protected State() {
      OnAttach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync += OnGamePausedSync;
        game.QuitRequested += OnGameQuitRequested;
        game.LoadRequested += OnGameLoadRequested;
        game.DeleteRequested += OnGameDeleteRequested;
      });
      OnDetach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync -= OnGamePausedSync;
        game.QuitRequested -= OnGameQuitRequested;
        game.LoadRequested -= OnGameLoadRequested;
        game.DeleteRequested -= OnGameDeleteRequested;
      });
    }

    private void OnGameDeleteRequested(string saveName) => Input(new Input.DeleteSave(saveName));
    private void OnGameLoadRequested(string saveName) => Input(new Input.Load(saveName));

    private void OnGameQuitRequested() => Input(new Input.OnQuitRequested());

    protected virtual void OnGamePausedSync(EPauseMode state)
      => Output(new Output.SetPauseMode(state != EPauseMode.NotPaused));

    public Transition On(in Input.Load input) {
      Get<Data>().SaveName = input.SaveName;
      return To<Loading>();
    }

    public Transition On(in Input.OnSaved input) {
      Get<IGameRepo>().BroadcastGameSavesUpdated();
      return ToSelf();
    }

    public Transition On(in Input.DeleteSave input) {
      Output(new Output.DeleteSave(input.SaveName));
      Get<IGameRepo>().BroadcastGameSavesUpdated();
      return ToSelf();
    }
  }
}
