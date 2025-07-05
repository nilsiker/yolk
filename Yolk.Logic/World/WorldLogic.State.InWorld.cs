namespace Yolk.Level;

using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.World;

public partial class WorldLogic {
  public abstract partial record State {
    public record InWorld : State, IGet<Input.Transition> {

      public InWorld() {
        OnAttach(() => Get<IWorldRepo>().Transitioning += OnWorldTransitioning);
        OnDetach(() => Get<IWorldRepo>().Transitioning -= OnWorldTransitioning);

        this.OnEnter(() => Get<IGameRepo>().BroadcastReady());
      }

      private void OnWorldTransitioning(string toLevelName) {
        var previousLevelName = Get<Data>().PreviousLevelName;
        Input(new Input.Transition(toLevelName, previousLevelName));
      }

      public Transition On(in Input.Transition input) {
        var data = Get<Data>();
        data.PreviousLevelName = input.FromLevelName;
        data.LevelToLoad = input.ToLevelName;
        return To<Transitioning>();
      }
    }
  }
}


