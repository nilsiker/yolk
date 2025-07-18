namespace Yolk.ExampleGame.Music;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.Music;

[Meta(typeof(IAutoNode))]
public partial class MusicReady : Node {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private AudioStream Music { get; set; } = default!;
  [Export] private float Crossfade { get; set; } = 2.0f;
  [Export] private float Delay { get; set; } = 0.0f;

  [Dependency] private IMusicRepo MusicRepo => this.DependOn<IMusicRepo>();

  public void OnResolved() {
    if (Music == null) {
      GD.PushWarning("Music is not set in MusicReady.");
      return;
    }

    MusicRepo.Start(Music.ResourcePath, Crossfade);
  }

}
