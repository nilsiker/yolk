
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      public partial record Alive {
        public partial record Grounded {
          [Meta, Id("playerlogic_state_enabled_alive_grounded_walking")]
          public partial record Walking : Grounded, IGet<Input.PhysicsTick> {
            public Walking() {
              this.OnEnter(() => Output(new Output.Animate("walk")));
            }
            public new Transition On(in Input.PhysicsTick input) {
              var data = Get<Data>();
              data.VelocityX = data.MoveDirectionX * data.Speed;
              Output(new Output.MoveAndSlide(data.VelocityX, data.VelocityY));
              if (data.MoveDirectionX > 0.0f) {
                Output(new Output.FaceRight());
              }
              else if (data.MoveDirectionX < 0.0f) {
                Output(new Output.FaceLeft());
              }
              return data.MoveDirectionX != 0.0f
                ? ToSelf()
                : To<Idle>();
            }
          }
        }
      }
    }
  }
}
