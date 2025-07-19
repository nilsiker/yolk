
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      public partial record Alive {
        [Meta, Id("playerlogic_state_enabled_alive_grounded")]
        public partial record Grounded : Alive {
        }
      }
    }
  }
}
