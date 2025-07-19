
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {
      public partial record Enabled {
        public partial record Grounded {
          [Meta, Id("playerlogic_state_alive_enabled_grounded_idle")]
          public partial record Idle : Grounded {
            public Idle() {
              this.OnEnter(() => Output(new Output.Animate("idle")));
            }
          }
        }
      }
    }
  }
}
