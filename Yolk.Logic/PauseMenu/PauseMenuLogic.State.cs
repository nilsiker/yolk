namespace Yolk;

using Chickensoft.LogicBlocks;
using Yolk.Game;


public partial class PauseMenuLogic {
  public partial record State : StateLogic<State>,
    IGet<Input.OnOptionsPressed>,
    IGet<Input.OnQuitPressed>,
    IGet<Input.OnResumePressed> {
    public State() {
      OnAttach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync += OnGamePausedSync;
        game.QuitRequested += OnGameQuitRequested;
      });
      OnDetach(() => {
        var game = Get<IGameRepo>();
        game.PauseMode.Sync -= OnGamePausedSync;
        game.QuitRequested -= OnGameQuitRequested;
      });
    }

    public Transition On(in Input.OnOptionsPressed input) {
      Get<IOptionsRepo>().SetUIVisible(true);
      return ToSelf();
    }

    public Transition On(in Input.OnResumePressed input) {
      Get<IGameRepo>().Resume();
      return ToSelf();
    }

    public Transition On(in Input.OnQuitPressed input) {
      if (input.QuitToDesktop) {
        Get<IAppRepo>().RequestQuit();
      }
      else {
        Get<IGameRepo>().RequestQuit();
      }
      return ToSelf();
    }

    private void OnGamePausedSync(EPauseMode state) {
      if (state == EPauseMode.PausedByPlayer) {
        Input(new Input.Show());
      }
      else {
        Input(new Input.Hide());
      }
    }

    private void OnGameQuitRequested() => Output(new Output.UpdateVisibility(false));
  }
}
