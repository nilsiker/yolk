namespace Yolk;

using Chickensoft.LogicBlocks;

public partial class MainMenuLogic {
  public partial record State {
    public partial record Hidden : State {
      public Hidden() {
        this.OnEnter(() => Output(new Output.UpdateVisibility(false)));
      }
    }
  }
}
