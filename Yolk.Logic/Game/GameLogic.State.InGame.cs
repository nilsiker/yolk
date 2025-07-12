namespace Yolk.Game;

using Chickensoft.LogicBlocks;
public partial class GameLogic {
  public abstract partial record State {
    public abstract partial record InGame : State, IGet<Input.OnQuitRequested>, IGet<Input.Autoload>, IGet<Input.OnGameOverTriggered> {
      protected InGame() {
        OnAttach(() => {
          var game = Get<IGameRepo>();
          game.SaveRequested += OnGameSaveRequested;
          game.AutosaveRequested += OnGameAutosaveRequested;
          game.AutoloadRequested += OnGameAutoloadRequested;
          game.GameOverRequested += OnGameOverRequested;

        });
        OnDetach(() => {
          var game = Get<IGameRepo>();
          game.SaveRequested -= OnGameSaveRequested;
          game.AutosaveRequested -= OnGameAutosaveRequested;
          game.AutoloadRequested -= OnGameAutoloadRequested;
          game.GameOverRequested -= OnGameOverRequested;
        });

        this.OnEnter(() => Get<IGameRepo>().BroadcastStarting());
      }

      private void OnGameAutosaveRequested() => Output(new Output.Autosave());
      private void OnGameSaveRequested(string saveName) => Output(new Output.SaveGame(saveName));
      private void OnGameAutoloadRequested() => Input(new Input.Autoload());
      private void OnGameOverRequested() => Input(new Input.OnGameOverTriggered());

      public Transition On(in Input.OnQuitRequested input) => To<Paused.Quitting>();
      public Transition On(in Input.OnGameOverTriggered input) => To<Paused.Over>();
      public Transition On(in Input.Autoload input) {
        Get<Data>().LoadType = ELoadType.Auto;
        return To<Loading>();
      }
    }
  }
}
