namespace Yolk.Level;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface IWorldLogic : ILogicBlock<WorldLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class WorldLogic : LogicBlock<WorldLogic.State>, IWorldLogic {
  public override Transition GetInitialState() => To<State.OutOfWorld>();

  public class Data {
    public string? PreviousLevelName;
    public required string LevelToLoad;
  }
}


