namespace Yolk.Level;

using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.World;

public partial class WorldLogic {
  public abstract partial record State {
    public record InWorld : State, IGet<Input.Transition>, IGet<Input.Exit> {

      public InWorld() {
        OnAttach(() => {
          Get<IWorldRepo>().Transitioning += OnWorldTransitioning;
          Get<IGameRepo>().Quitted += OnGameQuitted;
        });
        OnDetach(() => {
          Get<IWorldRepo>().Transitioning -= OnWorldTransitioning;
          Get<IGameRepo>().Quitted -= OnGameQuitted;
        });

        this.OnEnter(() => Get<IGameRepo>().BroadcastReady());
        this.OnExit(() => Output(new Output.Clear()));
      }

      private void OnGameQuitted() => Input(new Input.Exit());


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

      public Transition On(in Input.Exit input) => To<OutOfWorld>();
    }
  }
}


