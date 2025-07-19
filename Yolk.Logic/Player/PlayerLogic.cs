
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

[Meta, Id("playerlogic"), LogicBlock(typeof(State), Diagram = true)]
public partial class PlayerLogic : LogicBlock<PlayerLogic.State> {
  public override Transition GetInitialState() => To<State.Enabled.Alive.Grounded>();
}
