
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;
using Godot;
using Yolk.Logic.World;
using Yolk.World;

[Meta, LogicBlock(typeof(State), Diagram = true)]
public partial class PlayerLogic : LogicBlock<PlayerLogic.State> {
  public override Transition GetInitialState() => To<State.Disabled>();

  public abstract partial record State : StateLogic<State> {
    public State() {
      OnAttach(() => Get<IWorldRepo>().Transitioned += OnWorldLevelLoaded);
      OnDetach(() => Get<IWorldRepo>().Transitioned -= OnWorldLevelLoaded);
    }

    private void OnWorldLevelLoaded(Transform? entrypoint) {
      if (entrypoint is not null) {
        Output(new Output.Teleport(entrypoint));
      }
    }
  }
}
