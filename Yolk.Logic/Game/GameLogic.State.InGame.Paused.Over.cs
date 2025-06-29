namespace Yolk.Game;

using Chickensoft.LogicBlocks;

public partial class GameLogic {
  public abstract partial record State {
    public partial record InGame {
      public partial record Paused {
        public partial record Over : Paused, IGet<Input.OnQuitRequested> {
          public Over() {
            this.OnEnter(() => {
              var game = Get<IGameRepo>();
              game.BroadcastGameOver();
              game.Pause();
              Get<IAppRepo>().ReleaseMouse();
            });
            this.OnExit(() => Get<IAppRepo>().CaptureMouse());
          }
        }
      }
    }
  }
}
