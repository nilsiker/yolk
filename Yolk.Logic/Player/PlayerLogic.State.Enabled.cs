
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_enabled")]
    public partial record Enabled : State, IGet<Input.Kill> {
      public Transition On(in Input.Kill input) => To<Disabled>();
    }
  }
}
