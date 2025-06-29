namespace Yolk.UI;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Game;

public interface IPauseMenu : IControl, IStateInfo { }

[Meta(typeof(IAutoNode))]
public partial class PauseMenu : Control, IPauseMenu {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IOptionsRepo OptionsRepo => this.DependOn<IOptionsRepo>();

  [Node] private Button ResumeButton { get; set; } = default!;
  [Node] private Button OptionsButton { get; set; } = default!;
  [Node] private Button QuitMainMenuButton { get; set; } = default!;
  [Node] private Button QuitDesktopButton { get; set; } = default!;
  private PauseMenuLogic Logic { get; set; } = new();
  private PauseMenuLogic.IBinding Binding { get; set; } = default!;

  string IStateInfo.Name => Name;
  public string State => Logic.Value.ToString();


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
    OptionsButton.Pressed += OnOptionsButtonPressed;
    QuitMainMenuButton.Pressed += OnQuitMainMenuButtonPressed;
    QuitDesktopButton.Pressed += OnQuitDesktopButtonPressed;
  }

  private void OnResumeButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnResumePressed());
  private void OnOptionsButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnOptionsPressed());
  private void OnQuitMainMenuButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnQuitPressed(false));
  private void OnQuitDesktopButtonPressed() => Logic.Input(new PauseMenuLogic.Input.OnQuitPressed(true));

  public void _Ready() => AddToGroup("state");

  private void OnOutputUpdateVisibility(bool visible) {
    Visible = visible;
    if (visible) {
      ResumeButton.GrabFocus();
    }
  }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
