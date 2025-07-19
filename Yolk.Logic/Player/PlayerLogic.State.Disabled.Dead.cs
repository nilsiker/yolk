
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Disabled {
      [Meta, Id("playerlogic_state_disabled_dead")]
      public partial record Dead : Disabled {
        public Dead() {
          this.OnEnter(() => Get<IGameRepo>().RequestGameOver());
        }
      }
    }
  }
}
