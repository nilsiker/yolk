namespace Yolk.Game;

using Chickensoft.LogicBlocks;


public partial class GameLogic {
  public abstract partial record State {
    public partial record Loading : State, IGet<Input.Ready> {
      public Loading() {
        OnAttach(() => Get<IGameRepo>().Ready += OnGameReady);
        OnDetach(() => Get<IGameRepo>().Ready -= OnGameReady);

        this.OnEnter(() => {
          Get<IAppRepo>().RequestBlackout(OnBlackoutFinished);
          Get<IGameRepo>().Pause();
        });
        this.OnExit(() => Get<IGameRepo>().Pause());
      }

      private void OnGameReady() => Input(new Input.Ready());

      public Transition On(in Input.Ready input) => To<InGame.Playing>();

      public void OnBlackoutFinished() {
        var data = Get<Data>();
        switch (data.LoadType) {
          case ELoadType.Manual:
            Output(new Output.LoadGame(data.SaveName));
            break;
          case ELoadType.Auto:
            Output(new Output.Autoload());
            break;
          case ELoadType.Quick:
            Output(new Output.Quickload());
            break;
          default:
            break;
        }
      }
    }
  }
}
