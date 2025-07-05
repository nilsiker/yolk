namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public partial record Standby : State, IGet<Input.Start> {
      public Standby() {
        OnAttach(() => Get<IGameRepo>().Start += OnGameStartRequested);
        OnDetach(() => Get<IGameRepo>().Start -= OnGameStartRequested);

        this.OnEnter(() => Get<IGameRepo>().Pause());
      }

      private void OnGameStartRequested() => Input(new Input.Start());

      public Transition On(in Input.Start input) => To<Starting>();
    }
  }
}
