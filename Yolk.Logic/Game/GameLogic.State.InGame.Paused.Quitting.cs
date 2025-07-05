namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public partial record State {
    public abstract partial record InGame {
      public partial record Paused {
        public partial record Quitting : Paused, IGet<Input.QuittingTransitionFinished> {
          public Quitting() {
            this.OnEnter(() => {
              Output(new Output.QuitGame());
              Get<IAppRepo>().RequestBlackout(() => Input(new Input.QuittingTransitionFinished()));
            });
            this.OnExit(() => Get<IGameRepo>().BroadcastQuitted());
          }
          public Transition On(in Input.QuittingTransitionFinished input) => To<Standby>();
        }
      }
    }
  }
}
