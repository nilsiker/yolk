namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public partial record State {
    public abstract partial record InGame {
      public record Playing : InGame, IGet<Input.OnPauseUserInput> {
        public Playing() {
          OnAttach(() => {
            var game = Get<IGameRepo>();
            game.LoadRequested += OnGameLoadRequested;
          });
          OnDetach(() => {
            var game = Get<IGameRepo>();
            game.LoadRequested -= OnGameLoadRequested;
          });

          this.OnEnter(() => {
            var game = Get<IGameRepo>();

            game.Resume();

            Output(new Output.UpdateVisibility(true));
          });
        }

        private void OnGameLoadRequested(string saveName) => Output(new Output.LoadGame(saveName));
        public Transition On(in Input.OnPauseUserInput input) => To<Paused>();
      }
    }
  }
}
