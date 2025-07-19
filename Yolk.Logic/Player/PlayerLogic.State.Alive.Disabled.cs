
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;


public partial class PlayerLogic {
  public abstract partial record State {
    public partial record Alive {
      [Meta, Id("playerlogic_state_alive_disabled")]
      public partial record Disabled : Alive {
        public Disabled() {
          this.OnEnter(() => Output(new Output.SetEnabled(false)));
        }
      }
    }
  }
}

