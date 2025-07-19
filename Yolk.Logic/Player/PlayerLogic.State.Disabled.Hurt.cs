
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Disabled {
      [Meta, Id("playerlogic_state_enabled_alive_hurt")]
      public partial record Hurt : Disabled, IGet<Input.AnimationFinished>, IGet<Input.BlackoutFinished> {
        public Hurt() {
          this.OnEnter(() => Output(new Output.Animate("hurt")));
        }

        public Transition On(in Input.AnimationFinished input) {
          Get<IAppRepo>().RequestBlackout(() => Output(new Output.Teleport()));
          return ToSelf();
        }

        public Transition On(in Input.BlackoutFinished input) => To<Enabled.Alive>();
      }
    }
  }
}
