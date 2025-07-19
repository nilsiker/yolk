
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;


public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_enabled")]
    public partial record Enabled : State {
      public Enabled() {
        this.OnEnter(() => Output(new Output.SetEnabled(true)));
      }
    }
  }
}
