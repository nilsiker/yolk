namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public partial record Starting : State, IGet<Input.Ready> {
      public Starting() {
        OnAttach(() => Get<IGameRepo>().Ready += OnGameReady);
        OnDetach(() => Get<IGameRepo>().Ready -= OnGameReady);

        this.OnEnter(() => Get<IAppRepo>().RequestBlackout(() => Get<IGameRepo>().BroadcastStarting()));
      }

      private void OnGameReady() => Input(new Input.Ready());

      public Transition On(in Input.Ready input) => To<InGame.Playing>();
    }
  }
}
