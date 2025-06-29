namespace Yolk.Level;

using Yolk.World;

public partial class WorldLogic {
  public abstract partial record State {
    public record InLevel : State, IGet<Input.RequestLevelTransition> {

      public InLevel() {
        OnAttach(() => Get<IWorldRepo>().LevelLoadRequested += OnWorldLevelLoadRequested);
        OnDetach(() => Get<IWorldRepo>().LevelLoadRequested -= OnWorldLevelLoadRequested);
      }

      private void OnWorldLevelLoadRequested(string toLevelName) {
        var previousLevelName = Get<Data>().PreviousLevelName;
        Input(new Input.RequestLevelTransition(previousLevelName, toLevelName));
      }

      public Transition On(in Input.RequestLevelTransition input) {
        var data = Get<Data>();
        data.PreviousLevelName = input.FromLevelName;
        data.LevelToLoad = input.ToLevelName;
        data.SkipBlackout = input.SkipTransition;
        return To<LoadingLevel>();
      }
    }
  }
}


