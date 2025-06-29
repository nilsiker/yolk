namespace Yolk.Level;

using Yolk.Game;
using Yolk.World;

public partial class WorldLogic {
  public abstract partial record State {
    public record OutOfLevel : State, IGet<Input.RequestLevelTransition> {
      public OutOfLevel() {
        OnAttach(() => Get<IGameRepo>().Started += OnGameStarted);
        OnDetach(() => Get<IGameRepo>().Started -= OnGameStarted);
      }

      public Transition On(in Input.RequestLevelTransition input) {
        var data = Get<Data>();
        data.PreviousLevelName = input.FromLevelName;
        data.LevelToLoad = input.ToLevelName;
        data.SkipBlackout = input.SkipTransition;
        return To<LoadingLevel>();
      }

      private void OnGameStarted()
        => Input(new Input.RequestLevelTransition("0_DebugBox1", "0_DebugBox1"));
    }
  }
}


