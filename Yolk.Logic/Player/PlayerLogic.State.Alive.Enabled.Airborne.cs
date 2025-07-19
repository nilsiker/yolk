
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Godot;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {
      public partial record Enabled {
        [Meta, Id("playerlogic_state_alive_enabled_airborne")]
        public partial record Airborne : Enabled, IGet<Input.PhysicsTick>, IGet<Input.HitCeiling> {

          public Transition On(in Input.PhysicsTick input) {
            var data = Get<Data>();
            data.VelocityY += 9.82f * 30.0f * input.Delta;

            if (data.MoveDirectionX > 0.0f) {
              Output(new Output.FaceRight());
            }
            else if (data.MoveDirectionX < 0.0f) {
              Output(new Output.FaceLeft());
            }

            data.VelocityX = data.MoveDirectionX * data.Speed;
            GD.Print(data.VelocityX);
            Output(new Output.MoveAndSlide(data.VelocityX, data.VelocityY));
            return ToSelf();
          }


          public Transition On(in Input.HitCeiling input) {
            var data = Get<Data>();
            data.VelocityY = 0.0f;
            return To<Falling>();
          }
        }
      }
    }
  }
}
