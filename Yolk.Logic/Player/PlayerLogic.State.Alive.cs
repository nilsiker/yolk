
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {

  public abstract partial record State {
    [Meta, Id("playerlogic_state_alive")]
    public partial record Alive : State {
      public Alive() {
        OnAttach(() => Get<IPlayerRepo>().OutOfHearts += OnPlayerOutOfHearts);
        OnDetach(() => Get<IPlayerRepo>().OutOfHearts -= OnPlayerOutOfHearts);
      }

      private void OnPlayerOutOfHearts() => Input(new Input.Die());

      public Transition On(in Input.Die input) => To<Dead>();
    }
  }
}
