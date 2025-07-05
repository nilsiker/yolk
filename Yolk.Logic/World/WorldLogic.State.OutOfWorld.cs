namespace Yolk.Level;

using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class WorldLogic {
  public abstract partial record State {
    public record OutOfWorld : State, IGet<Input.Transition> {
      public OutOfWorld() {
        OnAttach(() => Get<IGameRepo>().Starting += OnGameStarting);
        OnDetach(() => Get<IGameRepo>().Starting -= OnGameStarting);

        this.OnEnter(() => Output(new Output.Clear()));
      }

      public Transition On(in Input.Transition input) {
        var data = Get<Data>();
        data.PreviousLevelName = input.FromLevelName;
        data.LevelToLoad = input.ToLevelName;
        return To<Transitioning>();
      }

      private void OnGameStarting()
        => Input(new Input.Transition("Level_0", null));
    }
  }
}
