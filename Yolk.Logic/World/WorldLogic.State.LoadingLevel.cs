namespace Yolk.Level;

using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.World;


public partial class WorldLogic {
  public abstract partial record State {
    public record LoadingLevel : State, IGet<Input.OnLevelLoaded> {
      public LoadingLevel() {
        this.OnEnter(() => {
          Get<IGameRepo>().Pause();
          if (Get<Data>().SkipBlackout) {
            LoadAndEnterLevel();
          }
          else {
            Get<IAppRepo>().RequestBlackout(LoadAndEnterLevel);
          }
        });
        this.OnExit(() => Get<IGameRepo>().Resume());
      }

      // TODO this breaks when quickloading when transitioning levels. Certain pause state for that, where quicksave/load is disabled?
      public Transition On(in Input.OnLevelLoaded input) {
        Get<IWorldRepo>().BroadcastLevelLoaded(input.LandingTransform);
        return To<InLevel>();
      }


      public void LoadAndEnterLevel() => Output(new Output.LoadLevel(Get<Data>().LevelToLoad));
    }
  }
}
