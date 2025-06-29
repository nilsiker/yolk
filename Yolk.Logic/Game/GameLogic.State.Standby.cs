namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public partial record Standby : State, IGet<Input.OnStartRequested> {
      public Standby() {
        OnAttach(() => Get<IGameRepo>().StartRequested += OnGameStartRequested);
        OnDetach(() => Get<IGameRepo>().StartRequested -= OnGameStartRequested);

        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(false));
          Get<IGameRepo>().Pause();
        });
      }

      private void OnGameStartRequested(int slot) => Input(new Input.OnStartRequested(slot));

      public Transition On(in Input.OnStartRequested input) {
        Get<Data>().Slot = input.Slot;
        return input.Slot < 0 ? To<InGame.Playing>() : To<Loading>();
      }
    }
  }
}
