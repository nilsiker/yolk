namespace Yolk.UI;

using Chickensoft.LogicBlocks;

public partial class OptionsMenuLogic {
  public abstract partial record State {
    public partial record Hidden : State, IGet<Input.Show> {
      public Hidden() {
        this.OnEnter(() => Output(new Output.UpdateVisibility(false)));
      }
      public Transition On(in Input.Show input) => To<Visible>();
    }
  }
}
