namespace Yolk;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface IPauseMenuLogic : ILogicBlock<PauseMenuLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class PauseMenuLogic
  : LogicBlock<PauseMenuLogic.State>,
    IPauseMenuLogic {
  public override Transition GetInitialState() => To<State.Hidden>();
}
