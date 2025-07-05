namespace Yolk.Level;

using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class WorldLogic {
  public abstract partial record State : StateLogic<State> {
  }
}


