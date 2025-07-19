
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      public partial record Alive {
        public partial record Grounded {
          [Meta, Id("playerlogic_state_enabled_alive_grounded_idle")]
          public partial record Idle : Grounded {

          }
        }
      }
    }
  }
}
