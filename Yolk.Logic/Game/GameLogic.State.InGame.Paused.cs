namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public partial record State {
    public abstract partial record InGame {
      public partial record Paused : InGame, IGet<Input.OnPauseUserInput> {
        public Paused() {
          OnAttach(() => Get<IGameRepo>().PauseMode.Changed += OnGamePausedChanged);
          OnDetach(() => Get<IGameRepo>().PauseMode.Changed -= OnGamePausedChanged);

          this.OnEnter(() => Get<IGameRepo>().Pause(true));
        }

        public Transition On(in Input.OnPauseUserInput input) => To<Playing>();

        private void OnGamePausedChanged(EPauseMode state) {
          if (state == EPauseMode.NotPaused) {
            Input(new Input.OnPauseUserInput());
          }
        }
      }
    }
  }
}
