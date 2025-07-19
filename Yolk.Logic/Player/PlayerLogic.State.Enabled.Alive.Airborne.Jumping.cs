
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      public partial record Alive {
        public partial record Airborne {
          [Meta, Id("playerlogic_state_enabled_alive_airborne_jumping")]
          public partial record Jumping : Airborne {

          }
        }
      }
    }
  }
}
