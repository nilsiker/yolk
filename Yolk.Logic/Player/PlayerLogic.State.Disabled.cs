
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_disabled")]
    public partial record Disabled : State {
      public Disabled() {
        this.OnEnter(() => Output(new Output.SetEnabled(false)));
      }

      [Meta, Id("playerlogic_state_enabled_alive_damaged")]
      public partial record Damaged : Disabled, IGet<Input.AnimationFinished>, IGet<Input.BlackoutFinished> {
        public Damaged() {
          this.OnEnter(() => Output(new Output.Animate("damaged")));
        }

        public Transition On(in Input.AnimationFinished input) {
          Get<IAppRepo>().RequestBlackout(() => Output(new Output.Teleport()));
          return ToSelf();
        }


        public Transition On(in Input.BlackoutFinished input) => To<Enabled.Alive>();
      }

      [Meta, Id("playerlogic_state_disabled_dead")]
      public partial record Dead : Disabled {
        public Dead() {
          this.OnEnter(() => Get<IGameRepo>().RequestGameOver());
        }
      }
    }
  }
}
