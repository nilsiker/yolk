namespace Yolk.Level;

using Chickensoft.LogicBlocks;

public partial class WorldLogic {
  public abstract partial record State : StateLogic<State> {
  }
}


