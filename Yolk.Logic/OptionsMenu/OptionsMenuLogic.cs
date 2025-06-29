namespace Yolk.UI;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class OptionsMenuLogic : LogicBlock<OptionsMenuLogic.State> {
  public override Transition GetInitialState() => To<State.Hidden>();
}
