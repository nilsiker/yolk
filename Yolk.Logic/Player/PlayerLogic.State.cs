
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {
  [Meta, Id("playerlogic_state")]
  public partial record State : StateLogic<State>, IGet<Input.RegisterCheckpoint> {
    public State() { }

    public Transition On(in Input.RegisterCheckpoint input) {
      var data = Get<Data>();
      data.CheckpointX = input.X;
      data.CheckpointY = input.Y;
      return ToSelf();
    }
  }
}
