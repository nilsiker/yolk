namespace Yolk.ExampleGame.Music;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.ExampleGame;
using Yolk.Logic.Music;

[Meta(typeof(IAutoNode))]
public partial class MusicArea2D : Area2D {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IMusicRepo MusicRepo => this.DependOn<IMusicRepo>();
  [Dependency] private AudioStream LevelMusic => this.DependOn<AudioStream>();

  [Export] private AudioStream MusicStream { get; set; } = default!;
  [Export] private float Crossfade { get; set; } = 1.0f;


  public void OnResolved() {
    BodyEntered += OnBodyEntered;
    BodyExited += OnBodyExited;
  }

  private void OnBodyEntered(Node2D body) {
    if (body is Player) {
      MusicRepo.Start(MusicStream.ResourcePath, Crossfade);
    }
  }

  private void OnBodyExited(Node2D body) {
    if (body is Player) {
      if (LevelMusic is not null) {
        MusicRepo.Start(LevelMusic.ResourcePath, Crossfade);
      }
      else {
        MusicRepo.Stop();
      }
    }
  }
}
