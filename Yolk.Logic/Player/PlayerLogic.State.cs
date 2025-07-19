
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.Logic.World;
using Yolk.World;

public partial class PlayerLogic {
  [Meta, Id("playerlogic_state")]
  public abstract partial record State : StateLogic<State>, IGet<Input.RegisterCheckpoint> {
    public State() {
      OnAttach(() => Get<IWorldRepo>().Transitioned += OnWorldLevelLoaded);
      OnDetach(() => Get<IWorldRepo>().Transitioned -= OnWorldLevelLoaded);
    }

    public Transition On(in Input.RegisterCheckpoint input) {
      var data = Get<Data>();
      data.CheckpointX = input.X;
      data.CheckpointY = input.Y;
      return ToSelf();
    }

    private void OnWorldLevelLoaded(Transform? entrypoint) {
      if (entrypoint is not null) {
        var data = Get<Data>();
        data.CheckpointX = entrypoint.Position.X;
        data.CheckpointY = entrypoint.Position.Y;

        Output(new Output.Teleport(entrypoint.Position.X, entrypoint.Position.Y));
        Get<IGameRepo>().BroadcastReady();  // Notify the game that the player is ready
      }
    }
  }
}
