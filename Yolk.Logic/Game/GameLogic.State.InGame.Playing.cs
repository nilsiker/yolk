namespace Yolk.Game;

using Chickensoft.LogicBlocks;


public partial class GameLogic {
  public partial record State {
    public abstract partial record InGame {
      public record Playing : InGame, IGet<Input.OnPauseUserInput>, IGet<Input.Quicksave>, IGet<Input.Quickload> {
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

        public Transition On(in Input.OnPauseUserInput input) => To<Paused>();
        public Transition On(in Input.Quickload input) => To<Loading>();
        public Transition On(in Input.Quicksave input) {
          Output(new Output.Quicksave());
          return ToSelf();
        }
      }
    }
  }
}
