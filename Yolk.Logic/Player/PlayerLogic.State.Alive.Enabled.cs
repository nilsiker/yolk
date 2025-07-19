
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;


public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Alive {

      [Meta, Id("playerlogic_state_alive_enabled")]
      public partial record Enabled : Alive,
        IGet<Input.Move>,
        IGet<Input.TakeDamage> {
        public Enabled() {
          this.OnEnter(() => Output(new Output.SetEnabled(true)));
        }


        public Transition On(in Input.TakeDamage input) {
          Get<IPlayerRepo>().Damage(input.Amount);
          return To<Disabled.Hurt>();
        }

        public Transition On(in Input.Move input) {
          var data = Get<Data>();
          data.MoveDirectionX = input.X;
          return ToSelf();
        }
      }
    }
  }
}
