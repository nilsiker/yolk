namespace Yolk;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface IMainMenuLogic : ILogicBlock<MainMenuLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class MainMenuLogic
  : LogicBlock<MainMenuLogic.State>,
    IMainMenuLogic {
  public override Transition GetInitialState() => To<State.Visible>();
}
