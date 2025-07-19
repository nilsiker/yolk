
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;


public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      [Meta, Id("playerlogic_state_enabled_alive")]
      public partial record Alive : Enabled, IGet<Input.TakeDamage>, IGet<Input.Die> {
        public Alive() {
          OnAttach(() => Get<IPlayerRepo>().OutOfHearts += OnPlayerOutOfHearts);
          OnDetach(() => Get<IPlayerRepo>().OutOfHearts -= OnPlayerOutOfHearts);
        }

        private void OnPlayerOutOfHearts() => Input(new Input.Die());


        public Transition On(in Input.TakeDamage input) {
          Get<IPlayerRepo>().Damage(input.Amount);
          return To<Disabled.Hurt>();
        }

        public Transition On(in Input.Die input) => To<Disabled.Dead>();
      }
    }
  }
}
