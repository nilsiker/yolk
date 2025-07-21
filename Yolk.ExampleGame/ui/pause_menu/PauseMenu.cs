namespace Yolk.UI;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Game;
using Yolk.Generator;

public interface IPauseMenu : IControl { }

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class PauseMenu : Control, IPauseMenu {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IOptionsRepo OptionsRepo => this.DependOn<IOptionsRepo>();

  [Node] private Button ResumeButton { get; set; } = default!;
  [Node] private Button SaveGameButton { get; set; } = default!;
  [Node] private Button OptionsButton { get; set; } = default!;
  [Node] private AnimationPlayer Anim { get; set; } = default!;

  [Node] private Button QuitMainMenuButton { get; set; } = default!;
  [Node] private Button QuitDesktopButton { get; set; } = default!;
  [Node] private Control SaveGamePanel { get; set; } = default!;
  private PauseMenuLogic Logic { get; set; } = new();
  private PauseMenuLogic.IBinding Binding { get; set; } = default!;


  public void OnResolved() {
    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
      .Handle((in PauseMenuLogic.Output.UpdateVisibility output) => OnOutputUpdateVisibility(output.Visible));

    Logic.Set(GameRepo);
    Logic.Set(AppRepo);
    Logic.Set(OptionsRepo);

    Logic.Start();

    ResumeButton.Pressed += OnResumeButtonPressed;
    SaveGameButton.Pressed += OnSaveGameButtonPressed;
    OptionsButton.Pressed += OnOptionsButtonPressed;
    QuitMainMenuButton.Pressed += OnQuitMainMenuButtonPressed;
    QuitDesktopButton.Pressed += OnQuitDesktopButtonPressed;
  }

  private void OnSaveGameButtonPressed() => SaveGamePanel.Visible = true;
  private void OnResumeButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnResumePressed());
  private void OnOptionsButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnOptionsPressed());
  private void OnQuitMainMenuButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnQuitPressed(false));
  private void OnQuitDesktopButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnQuitPressed(true));

  private void OnOutputUpdateVisibility(bool visible) {
    Visible = visible;
    if (visible) {
      Anim.Play("show");
      ResumeButton.GrabFocus();
    }
    else {
      Anim.Play("hide");
    }
  }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
