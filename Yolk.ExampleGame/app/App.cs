namespace Yolk.App;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Generator;
using Yolk.Options;


public interface IApp : INode, IProvide<IAppRepo>, IProvide<IOptionsRepo> {
  public void OnOutputQuitApp();
}

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class App : Node, IApp {
  public override void _Notification(int what) => this.Notify(what);

  [Node] private IOptions Options { get; set; } = default!;
  [Node] private AnimationPlayer Blackout { get; set; } = default!;

  private IAppRepo AppRepo { get; set; } = new AppRepo();
  private AppLogic Logic { get; set; } = new();
  private AppLogic.IBinding Binding { get; set; } = default!;

  IAppRepo IProvide<IAppRepo>.Value() => AppRepo;
  IOptionsRepo IProvide<IOptionsRepo>.Value() => Options.OptionsRepo;

  public void OnResolved() {
    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
      .Handle((in AppLogic.Output.SetMouseCaptured output) => OnOutputSetMouseCaptured(output.Captured))
      .Handle((in AppLogic.Output.SetBlackout output) => OnOutputSetBlackout(output.On))
      .Handle((in AppLogic.Output.QuitApp output) => OnOutputQuitApp());

    Logic.Set(new AppLogic.Data {
      Callback = null
    });
    Logic.Set(AppRepo);

    Logic.Start();

    Blackout.AnimationFinished += OnBlackoutAnimationFinished;

    this.Provide();
  }

  private void OnBlackoutAnimationFinished(StringName animName) => Logic.Input(new AppLogic.Input.BlackoutFinished());


  private void OnOutputSetBlackout(bool on) => Blackout.Play(on ? "fade_to_black" : "fade_from_black");


  private static void OnOutputSetMouseCaptured(bool captured)
    => Input.MouseMode = captured
      ? Input.MouseModeEnum.Captured
      : Input.MouseModeEnum.Visible;
  public void OnOutputQuitApp() => GetTree().Quit();

  public override void _Input(InputEvent @event) {
    if (@event is InputEventMouseMotion && Input.MouseMode == Input.MouseModeEnum.Visible) {
      Logic.Input(new AppLogic.Input.ReleasedMouseMotionOccurred(GetViewport().GetMousePosition()));
    }
  }

  public override void _ExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
