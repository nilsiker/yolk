namespace Yolk;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Game;
using Yolk.Generator;
using Yolk.Logic.SoundEffects;


public interface IMainMenu : IControl {
}

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class MainMenu : Control, IMainMenu {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();
  [Dependency] private IOptionsRepo Options => this.DependOn<IOptionsRepo>();

  [Node] private Button PlayButton { get; set; } = default!;
  [Node] private Button LoadButton { get; set; } = default!;
  [Node] private Button OptionsButton { get; set; } = default!;
  [Node] private Button QuitButton { get; set; } = default!;
  [Node] private Control SaveGamePanel { get; set; } = default!;

  private MainMenuLogic Logic { get; set; } = new();
  private MainMenuLogic.IBinding Binding { get; set; } = default!;

  public void OnResolved() {
    Binding = Logic.Bind();
    Binding.Handle((in MainMenuLogic.Output.UpdateVisibility output) => Visible = output.Visible);

    // Bind functions to state outputs here
    PlayButton.Pressed += OnPlayButtonPressed;
    LoadButton.Pressed += OnLoadButtonPressed;
    OptionsButton.Pressed += OnOptionsButtonPressed;
    QuitButton.Pressed += OnQuitButtonPressed;

    PlayButton.FocusEntered += OnPlayButtonFocused;
    LoadButton.FocusEntered += OnLoadButtonFocused;
    OptionsButton.FocusEntered += OnOptionsButtonFocused;
    QuitButton.FocusEntered += OnQuitButtonFocused;

    PlayButton.FocusExited += OnPlayButtonUnfocused;
    LoadButton.FocusExited += OnLoadButtonUnfocused;
    OptionsButton.FocusExited += OnOptionsButtonUnfocused;
    QuitButton.FocusExited += OnQuitButtonUnfocused;

    Logic.Set(GameRepo);
    Logic.Set(AppRepo);

    Logic.Start();

    PlayButton.GrabFocus();
  }

  private void OnAppSetMainMenuVisibility(bool visible) => Visible = visible;
  private void OnPlayButtonFocused() => PlayButton.Text = "> Play";
  private void OnPlayButtonUnfocused() => PlayButton.Text = "Play";
  private void OnPlayButtonPressed() => GameRepo.RequestStart();

  private void OnLoadButtonUnfocused() => LoadButton.Text = "Load Game";
  private void OnLoadButtonFocused() => LoadButton.Text = "> Load Game";
  private void OnLoadButtonPressed() => SaveGamePanel.Visible = true;

  private void OnOptionsButtonFocused() => OptionsButton.Text = "> Options";
  private void OnOptionsButtonUnfocused() => OptionsButton.Text = "Options";
  private void OnOptionsButtonPressed() => Options.SetUIVisible(true);

  private void OnQuitButtonFocused() => QuitButton.Text = "> Quit";
  private void OnQuitButtonUnfocused() => QuitButton.Text = "Quit";
  private void OnQuitButtonPressed() => Logic.Input(new MainMenuLogic.Input.OnQuitButtonPressed());


  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
