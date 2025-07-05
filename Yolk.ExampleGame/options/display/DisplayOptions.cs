namespace Yolk.Options;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode))]
public partial class DisplayOptions : Node {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IOptionsRepo OptionsRepo => this.DependOn<IOptionsRepo>();

  public void OnResolved() {

    OptionsRepo.Fullscreen.Sync += OnOptionsFullscreenSync;
    OptionsRepo.Vsync.Sync += OnOptionsVsyncSync;
  }

  private static void OnOptionsFullscreenSync(bool fullscreen) => DisplayServer.Singleton.WindowSetMode(fullscreen
    ? DisplayServer.WindowMode.Fullscreen
    : DisplayServer.WindowMode.Windowed
  );

  private static void OnOptionsVsyncSync(bool vsync) => DisplayServer.Singleton.WindowSetVsyncMode(vsync
    ? DisplayServer.VSyncMode.Enabled
    : DisplayServer.VSyncMode.Disabled
  );
}
