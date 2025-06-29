namespace Yolk;

using Chickensoft.LogicBlocks;
using Yolk.Game;

public partial class MainMenuLogic {
  public partial record State : StateLogic<State>,
    IGet<Input.OnQuitButtonPressed>,
    IGet<Input.Hide>,
    IGet<Input.Show> {
    public State() {
      OnAttach(() => {
        var game = Get<IGameRepo>();
        game.Started += OnGameStarted;
        game.Quitted += OnGameQuitted;
      });
      OnDetach(() => {
        var game = Get<IGameRepo>();
        game.Started -= OnGameStarted;
        game.Quitted -= OnGameQuitted;
      });
    }

    private void OnGameQuitted() => Input(new Input.Show());
    private void OnGameStarted() => Input(new Input.Hide());

    public Transition On(in Input.OnQuitButtonPressed input) {
      Get<IAppRepo>().RequestQuit();
      return ToSelf();
    }

    public Transition On(in Input.Show input) => To<Visible>();
    public Transition On(in Input.Hide input) => To<Hidden>();
  }
}
