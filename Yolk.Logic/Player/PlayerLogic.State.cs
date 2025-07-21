
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Yolk.Game;
using Yolk.Logic.World;
using Yolk.World;

public partial class PlayerLogic {
  [Meta, Id("playerlogic_state")]
  public partial record State : StateLogic<State>, IGet<Input.RegisterCheckpoint>, IGet<Input.OnGameStarting> {
    public State() {
      OnAttach(() => {
        Get<IGameRepo>().Starting += OnGameStarting;
        Get<IWorldRepo>().Transitioned += OnWorldLevelLoaded;
      });
      OnDetach(() => {
        Get<IGameRepo>().Starting -= OnGameStarting;
        Get<IWorldRepo>().Transitioned -= OnWorldLevelLoaded;
      });
    }

    private void OnGameStarting() => Input(new Input.OnGameStarting());


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

    public Transition On(in Input.OnGameStarting input) {
      var player = Get<IPlayerRepo>();

      // TODO refactor into settings class or something
      player.SetMaxHealth(3);
      player.SetMaxCharges(1);
      player.SetHealth(3);
      player.SetCharges(1);

      return To<Alive.Enabled.Grounded.Idle>();
    }
  }
}
