
namespace Yolk.ExampleGame.UI.HUD;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Game;

[Meta(typeof(IAutoNode))]
public partial class HUD : Control {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private AnimationPlayer AnimationPlayer { get; set; } = default!;

  public void OnResolved() => GameRepo.PauseMode.Sync += OnGamePauseModeSync;

  private void OnGamePauseModeSync(EPauseMode mode) {
    if (mode == EPauseMode.NotPaused) {
      Visible = true;
      AnimationPlayer.Play("show");
    }

    else {
      AnimationPlayer.Play("hide");
    }
  }
}
