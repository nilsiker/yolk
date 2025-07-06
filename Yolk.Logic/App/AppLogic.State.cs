namespace Yolk;

using Chickensoft.LogicBlocks;

public partial class AppLogic {
  public partial record State : StateLogic<State>, IGet<Input.BlackoutRequested> {
    public State() {
      OnAttach(() => {
        var app = Get<IAppRepo>();
        app.QuitRequested += OnAppQuitRequested;
        app.MouseReleases.Sync += OnAppMouseReleasesSync;
        app.BlackoutRequested += OnBlackoutRequested;
      });
      OnDetach(() => {
        var app = Get<IAppRepo>();
        app.QuitRequested -= OnAppQuitRequested;
        app.MouseReleases.Sync -= OnAppMouseReleasesSync;
        app.BlackoutRequested -= OnBlackoutRequested;
      });
    }

    private void OnBlackoutRequested(BlackoutCallback callback) => Input(new Input.BlackoutRequested(callback));
    private void OnAppQuitRequested() => Output(new Output.QuitApp());
    private void OnAppMouseReleasesSync(int releases)
      => Output(new Output.SetMouseCaptured(releases == 0));

    public Transition On(in Input.BlackoutRequested input) {
      Get<Data>().Callback = input.Callback;
      return To<BlackingOut>();
    }

    public partial record BlackingOut : State, IGet<Input.BlackoutFinished> {
      public BlackingOut() {
        this.OnEnter(() => Output(new Output.SetBlackout(true)));
        this.OnExit(() => {
          var data = Get<Data>();

          var minimumDelay = Task.Delay(data.BlackoutMinimumWaitTimeMs);

          if (data.Callback is not null) {
            data.Callback();
          }

          Task.WaitAny(minimumDelay);

          Output(new Output.SetBlackout(false));
        });
      }

      public Transition On(in Input.BlackoutFinished input) => To<State>();
    }
  }
}
