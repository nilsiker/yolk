
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.Logic.World;
using Yolk.World;

public partial class PlayerLogic {
  [Meta, Id("playerlogic_state")]
  public abstract partial record State : StateLogic<State> {
    public State() {
      OnAttach(() => Get<IWorldRepo>().Transitioned += OnWorldLevelLoaded);
      OnDetach(() => Get<IWorldRepo>().Transitioned -= OnWorldLevelLoaded);
    }

    private void OnWorldLevelLoaded(Transform? entrypoint) {
      if (entrypoint is not null) {
        Output(new Output.Teleport(entrypoint));
        Get<IGameRepo>().BroadcastReady();  // Notify the game that the player is ready
      }
    }
  }
}
