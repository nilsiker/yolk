namespace Yolk.Level;

using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.World;

public partial class WorldLogic {
  public abstract partial record State {
    public record Transitioning : State, IGet<Input.OnTransitioned> {
      public Transitioning() {
        this.OnEnter(() => {
          Get<IGameRepo>().Pause();
          Output(new Output.TransitionLevel(Get<Data>().LevelToLoad));
        });
        this.OnExit(() => Get<IGameRepo>().Resume());

      }

      public Transition On(in Input.OnTransitioned input) {
        Get<IWorldRepo>().BroadcastTransitioned(input.Entrypoint);
        return To<InWorld>();
      }

      public void UnloadLevel() {
      }
    }
  }
}
