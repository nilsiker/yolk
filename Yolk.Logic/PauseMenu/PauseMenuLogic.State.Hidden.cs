namespace Yolk;

using Chickensoft.LogicBlocks;

public partial class PauseMenuLogic {
  public partial record State {
    public partial record Hidden : State, IGet<Input.Show> {
      public Hidden() {
        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(false));
          Get<IOptionsRepo>().SetUIVisible(false);
        });
      }

      public Transition On(in Input.Show input) => To<Visible>();
    }
  }
}
