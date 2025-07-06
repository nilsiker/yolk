
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_disabled")]
    public partial record Disabled : State {
      public Disabled() {
        this.OnEnter(() => Get<IGameRepo>().Autoload());
      }
    }
  }
}
