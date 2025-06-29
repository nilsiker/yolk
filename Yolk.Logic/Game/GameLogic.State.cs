namespace Yolk.Game;

using Chickensoft.LogicBlocks;
public partial class GameLogic {
  public abstract partial record State : StateLogic<State>, IGet<Input.Load> {
    protected State() {
      OnAttach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync += OnGamePausedSync;
        game.QuitRequested += OnGameQuitRequested;
        game.Slot.Changed += OnGameSlotSync;
      });
      OnDetach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync -= OnGamePausedSync;
        game.QuitRequested -= OnGameQuitRequested;
        game.Slot.Changed -= OnGameSlotSync;
      });
    }

    private void OnGameSlotSync(int slot) => Output(new Output.SetSlot(slot));
    private void OnGameQuitRequested() => Input(new Input.OnQuitRequested());

    protected virtual void OnGamePausedSync(EPauseMode state)
      => Output(new Output.SetPauseMode(state != EPauseMode.NotPaused));

    public Transition On(in Input.Load input) {
      Get<Data>().Slot = input.Slot;
      return To<Loading>();
    }
  }
}
