namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public abstract partial record InGame : State, IGet<Input.OnQuitRequested>, IGet<Input.Save>, IGet<Input.OnGameOverTriggered> {
      protected InGame() {
        OnAttach(() => {
          var game = Get<IGameRepo>();
          game.SaveRequested += OnGameSaveRequested;
          game.GameOverRequested += OnGameOverRequested;

        });
        OnDetach(() => {
          var game = Get<IGameRepo>();
          game.SaveRequested -= OnGameSaveRequested;
          game.GameOverRequested -= OnGameOverRequested;
        });

        this.OnEnter(() => Get<IGameRepo>().BroadcastStarting());
      }

      private void OnGameOverRequested() => Input(new Input.OnGameOverTriggered());
      private void OnGameSaveRequested(string saveName) => Output(new Output.SaveGame(saveName));

      public Transition On(in Input.OnQuitRequested input) => To<Paused.Quitting>();
      public Transition On(in Input.Save input) {
        Get<IGameRepo>().Save(input.SaveName, input.SaveType);
        return ToSelf();
      }

      public Transition On(in Input.OnGameOverTriggered input) => To<Paused.Over>();
    }
  }
}
