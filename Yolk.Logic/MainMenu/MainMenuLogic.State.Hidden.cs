namespace Yolk;

using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class MainMenuLogic {
  public partial record State {
    public partial record Hidden : State, IGet<Input.Show> {
      public Hidden() {
        OnAttach(() => Get<IGameRepo>().Quitted += OnGameQuitted);
        OnDetach(() => Get<IGameRepo>().Quitted -= OnGameQuitted);

        this.OnEnter(() => Output(new Output.UpdateVisibility(false)));
      }

      private void OnGameQuitted() => Input(new Input.Show());

      public Transition On(in Input.Show input) => To<Visible>();
    }
  }
}
