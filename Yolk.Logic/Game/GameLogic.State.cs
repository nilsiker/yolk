namespace Yolk.Game;

using Chickensoft.LogicBlocks;
using Yolk.FS;


public partial class GameLogic {
  public abstract partial record State : StateLogic<State>, IGet<Input.Load>, IGet<Input.OnSaved> {
    protected State() {
      OnAttach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync += OnGamePausedSync;
        game.QuitRequested += OnGameQuitRequested;
        game.LastSaveName.Sync += OnGameLastSaveNameSync;
        game.LoadRequested += OnGameLoadRequested;
      });
      OnDetach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync -= OnGamePausedSync;
        game.QuitRequested -= OnGameQuitRequested;
        game.LastSaveName.Sync -= OnGameLastSaveNameSync;
        game.LoadRequested -= OnGameLoadRequested;
      });
    }

    private void OnGameLoadRequested(string saveName) => Input(new Input.Load(saveName, ESaveType.Manual));
    private void OnGameLastSaveNameSync(string? saveName) => Output(new Output.SetLastSaveName(saveName));
    private void OnGameQuitRequested() => Input(new Input.OnQuitRequested());

    protected virtual void OnGamePausedSync(EPauseMode state)
      => Output(new Output.SetPauseMode(state != EPauseMode.NotPaused));

    public Transition On(in Input.Load input) {
      Get<Data>().LastSaveName = input.SaveName;
      return To<Loading>();
    }

    public Transition On(in Input.OnSaved input) {
      Get<IGameRepo>().BroadcastSaved();
      return ToSelf();
    }
  }
}
