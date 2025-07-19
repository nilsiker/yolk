
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_disabled")]
    public partial record Disabled : State {
      public Disabled() {
        this.OnEnter(() => Output(new Output.SetEnabled(false)));
      }
    }
  }
}
