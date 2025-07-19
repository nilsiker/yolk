
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {
      public partial record Enabled {
        public partial record Airborne {
          [Meta, Id("playerlogic_state_alive_enabled_airborne_falling")]
          public partial record Falling : Airborne,
            IGet<Input.OnGrounded>,
            IGet<Input.Jump> {
            public Falling() {
              this.OnEnter(() => {
                Output(new Output.Animate("jump"));
                var data = Get<Data>();
              });
            }

            public Transition On(in Input.Jump input) {
              var player = Get<IPlayerRepo>();

              if (player.Charges.Value > 0) {
                player.RemoveCharge();
                return To<Jumping>();
              }

              return ToSelf();
            }

            public Transition On(in Input.OnGrounded input) => input.IsGrounded
              ? To<Grounded.Idle>()
              : ToSelf();
          }
        }
      }
    }
  }
}
