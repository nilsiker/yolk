
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      public partial record Alive {
        public partial record Airborne {
          [Meta, Id("playerlogic_state_enabled_alive_airborne_jumping")]
          public partial record Jumping : Airborne, IGet<Input.StopJump>, IGet<Input.PhysicsTick> {
            private float _jumpTimer;

            public Jumping() {
              this.OnEnter(() => {
                _jumpTimer = 0.0f;
                var data = Get<Data>();
                data.VelocityY = -data.Speed;
                Output(new Output.Animate("jump"));
                Output(new Output.OnJump());
              });
            }

            public Transition On(in Input.StopJump input) => To<Falling>();
            public new Transition On(in Input.PhysicsTick input) {
              _jumpTimer += input.Delta;

              var data = Get<Data>();
              Output(new Output.MoveAndSlide(data.VelocityX, data.VelocityY));
              return _jumpTimer < 0.25f // Allow jumping for a short duration
                ? ToSelf()
                : To<Falling>(); // Transition to Falling state after the jump duration
            }
          }
        }
      }
    }
  }
}
