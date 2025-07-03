namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public partial record Standby : State, IGet<Input.OnStartRequested> {
      public Standby() {
        OnAttach(() => Get<IGameRepo>().StartRequested += OnGameStartRequested);
        OnDetach(() => Get<IGameRepo>().StartRequested -= OnGameStartRequested);

        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(true));
          Get<IGameRepo>().Pause();
        });
      }

      private void OnGameStartRequested() => Get<IAppRepo>().RequestBlackout(() => Input(new Input.OnStartRequested()));

      public Transition On(in Input.OnStartRequested input) => To<InGame.Playing>();
    }
  }
}
