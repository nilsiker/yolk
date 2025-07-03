namespace Yolk;

using Chickensoft.LogicBlocks;

public partial class MainMenuLogic {
  public partial record State : StateLogic<State>,
    IGet<Input.OnQuitButtonPressed> {
    public State() { }

    public Transition On(in Input.OnQuitButtonPressed input) {
      Get<IAppRepo>().RequestQuit();
      return ToSelf();
    }
  }
}
