
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_disabled_dead")]
    public partial record Dead : State {
      public Dead() {
        this.OnEnter(() => Get<IGameRepo>().RequestGameOver());
      }
    }
  }
}
