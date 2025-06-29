namespace Yolk.Level;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface ILevelLogic : ILogicBlock<WorldLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class WorldLogic : LogicBlock<WorldLogic.State>, ILevelLogic {
  public override Transition GetInitialState() => To<State.OutOfLevel>();

  public class Data {
    public required string PreviousLevelName;
    public required string LevelToLoad;
    public required bool SkipBlackout;
  }
}


