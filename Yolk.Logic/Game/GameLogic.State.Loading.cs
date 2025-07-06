namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public partial record Loading : State, IGet<Input.OnLoaded> {
      public Loading() {
        this.OnEnter(() => {
          Get<IGameRepo>().Pause();
          Get<IAppRepo>().RequestBlackout(OnBlackoutFinished!);
        });
        this.OnExit(() => Get<IGameRepo>().Pause());
      }

      public Transition On(in Input.OnLoaded input) => To<InGame.Playing>();

      public void OnBlackoutFinished() => Output(new Output.LoadGame(Get<Data>().LastSaveName));
    }
  }
}
