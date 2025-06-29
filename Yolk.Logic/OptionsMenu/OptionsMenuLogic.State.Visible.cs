namespace Yolk.UI;

using Chickensoft.LogicBlocks;

public partial class OptionsMenuLogic {
  public abstract partial record State {
    public partial record Visible : State, IGet<Input.Hide> {
      public Visible() {
        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(true));
          Get<IAppRepo>().ReleaseMouse();
        });
        this.OnExit(() => Get<IAppRepo>().CaptureMouse());
      }

      public Transition On(in Input.Hide input) => To<Hidden>();
    }
  }
}
