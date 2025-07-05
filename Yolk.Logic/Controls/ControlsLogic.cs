
namespace Yolk.Logic.Controls;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

[Meta, LogicBlock(typeof(State), Diagram = true)]
public partial class ControlsLogic : LogicBlock<ControlsLogic.State> {
  public override Transition GetInitialState() => To<State.Idle>();
  public abstract partial record State : StateLogic<State>;
}

