
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {
      public partial record Enabled {
        [Meta]
        public abstract partial record Grounded : Enabled, IGet<Input.OnGrounded>, IGet<Input.Jump>, IGet<Input.PhysicsTick> {
          public Grounded() {
            this.OnEnter(() => {
              Get<Data>().VelocityY = 0.0f;
              var player = Get<IPlayerRepo>();
              player.AddCharge(player.MaxCharges.Value);
            });

          }
          public Transition On(in Input.OnGrounded input)
            => input.IsGrounded ? ToSelf() : To<Airborne.Falling>();

          public Transition On(in Input.Jump input) => To<Airborne.Jumping>();
          public Transition On(in Input.PhysicsTick input) {
            Output(new Output.MoveAndSlide(Get<Data>().MoveDirectionX * Get<Data>().Speed, 0.0f));
            return Get<Data>().MoveDirectionX != 0.0f ? To<Walking>() : To<Idle>();
          }
        }
      }
    }
  }
}
