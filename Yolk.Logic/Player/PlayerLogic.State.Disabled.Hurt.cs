
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Disabled {
      [Meta, Id("playerlogic_state_enabled_alive_hurt")]
      public partial record Hurt : Disabled,
        IGet<Input.AnimationFinished>,
        IGet<Input.BlackoutFinished>,
        IGet<Input.PhysicsTick> {
        public Hurt() {
          this.OnEnter(() => {
            Output(new Output.Animate("hurt"));
            Get<Data>().VelocityY = -125.0f; // Jump up a bit
            Get<Data>().VelocityX = 0.0f; // Stop horizontal movement
          });
        }

        public Transition On(in Input.AnimationFinished input) {
          Get<IAppRepo>().RequestBlackout(() => Output(new Output.Teleport()));
          return ToSelf();
        }

        public Transition On(in Input.BlackoutFinished input) => To<Enabled.Alive>();

        public Transition On(in Input.PhysicsTick input) {
          var data = Get<Data>();
          data.VelocityY += 9.82f * 30.0f * input.Delta;
          Output(new Output.MoveAndSlide(data.VelocityX, data.VelocityY));
          return ToSelf();
        }
      }
    }
  }
}
