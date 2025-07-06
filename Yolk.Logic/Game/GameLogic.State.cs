namespace Yolk.Game;

using Chickensoft.LogicBlocks;
public partial class GameLogic {
  public abstract partial record State : StateLogic<State>, IGet<Input.Load> {
    protected State() {
      OnAttach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync += OnGamePausedSync;
        game.QuitRequested += OnGameQuitRequested;
        game.LastSaveName.Sync += OnGameLastSaveNameSync;
      });
      OnDetach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync -= OnGamePausedSync;
        game.QuitRequested -= OnGameQuitRequested;
        game.LastSaveName.Sync -= OnGameLastSaveNameSync;
      });
    }

    private void OnGameLastSaveNameSync(string? saveName) => Output(new Output.SetLastSaveName(saveName));
    private void OnGameQuitRequested() => Input(new Input.OnQuitRequested());

    protected virtual void OnGamePausedSync(EPauseMode state)
      => Output(new Output.SetPauseMode(state != EPauseMode.NotPaused));

    public Transition On(in Input.Load input) {
      Get<Data>().LastSaveName = input.SaveName;
      return To<Loading>();
    }
  }
}
