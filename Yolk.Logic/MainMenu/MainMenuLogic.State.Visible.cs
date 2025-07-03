namespace Yolk;

using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class MainMenuLogic {
  public partial record State {
    public partial record Visible : State, IGet<Input.Hide> {
      public Visible() {
        OnAttach(() => Get<IGameRepo>().Started += OnGameStarted);
        OnDetach(() => Get<IGameRepo>().Started -= OnGameStarted);

        this.OnEnter(() => {
          Output(new Output.UpdateVisibility(true));
          Get<IAppRepo>().ReleaseMouse();
        });

        this.OnExit(() => Get<IAppRepo>().CaptureMouse());
      }

      private void OnGameStarted() => Input(new Input.Hide());

      public Transition On(in Input.Hide input) => To<Hidden>();
    }
  }
}
