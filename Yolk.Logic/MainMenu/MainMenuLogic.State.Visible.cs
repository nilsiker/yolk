namespace Yolk;

using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class MainMenuLogic {
  public partial record State {
    public partial record Visible : State, IGet<Input.Hide> {
      public Visible() {
        OnAttach(() => Get<IGameRepo>().Starting += OnGameStarting);
        OnDetach(() => Get<IGameRepo>().Starting -= OnGameStarting);

        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(true));
          Get<IAppRepo>().ReleaseMouse();
        });

        this.OnExit(() => Get<IAppRepo>().CaptureMouse());
      }

      private void OnGameStarting() => Input(new Input.Hide());

      public Transition On(in Input.Hide input) => To<Hidden>();
    }
  }
}
