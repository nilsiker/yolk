namespace Yolk;

using Chickensoft.LogicBlocks;

public partial class MainMenuLogic {
  public partial record State {
    public partial record Visible : State {
      public Visible() {
        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(true));
          Get<IAppRepo>().ReleaseMouse();
        });

        this.OnExit(() => Get<IAppRepo>().CaptureMouse());
      }
    }
  }
}
