
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {
      public partial record Disabled {
        [Meta, Id("playerlogic_state_alive_disabled_hurt")]
        public partial record Hurt : Disabled,
          IGet<Input.AnimationFinished>,
          IGet<Input.BlackoutFinished>,
          IGet<Input.PhysicsTick>,
          IGet<Input.OnGrounded> {
          public Hurt() {
            this.OnEnter(() => {
              Output(new Output.Animate("hurt"));
              Get<Data>().VelocityY = -125.0f;
              Get<Data>().VelocityX /= 2;
            });

            this.OnExit(() => Output(new Output.GrantInvincibility(1.5f)));
          }

          public Transition On(in Input.AnimationFinished input) {
            var data = Get<Data>();

            Get<IAppRepo>().RequestBlackout(() => {
              Output(new Output.Teleport(data.CheckpointX, data.CheckpointY));
              Output(new Output.MoveAndSlide(data.VelocityX, data.VelocityY));
              Input(new Input.BlackoutFinished());
            });

            return ToSelf();
          }

          public Transition On(in Input.BlackoutFinished input) {
            var data = Get<Data>();

            var dead = Get<IPlayerRepo>().Hearts.Value == 0;

            return dead ? To<Dead>() : To<Enabled.Grounded.Idle>();
          }

          public Transition On(in Input.PhysicsTick input) {
            var data = Get<Data>();
            data.VelocityY += 9.82f * 30.0f * input.Delta;
            Output(new Output.MoveAndSlide(data.VelocityX, data.VelocityY));
            return ToSelf();
          }

          public Transition On(in Input.OnGrounded input) {
            var data = Get<Data>();
            if (input.IsGrounded && data.VelocityY > 0.0f) {
              data.VelocityY = 0.0f;
              data.VelocityX = 0.0f;
            }
            return ToSelf();
          }
        }
      }
    }
  }
}
