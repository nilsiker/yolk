namespace Yolk.ExampleGame.UI;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Game;


[Meta(typeof(IAutoNode))]
public partial class GameOverMenu : Control {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private Button RetryButton { get; set; } = default!;
  [Node] private Button QuitButton { get; set; } = default!;
  [Node] private AnimationPlayer Anim { get; set; } = default!;

  public void OnResolved() {
    RetryButton.Pressed += OnRetryButtonPressed;
    QuitButton.Pressed += OnQuitButtonPressed;

    GameRepo.GameOver += OnGameOver;
    GameRepo.Quitted += OnQuitted;
    GameRepo.Ready += OnGameReady;
    GameRepo.LoadRequested += OnLoadRequested;
  }

  private void OnGameReady() => Visible = false;
  private void OnLoadRequested(string obj) => Visible = false;
  private void OnQuitted() => Visible = false;
  private void OnGameOver() => Visible = true;
  private void OnRetryButtonPressed() => GameRepo.Autoload();
  private void OnQuitButtonPressed() => GameRepo.RequestQuit();

}
