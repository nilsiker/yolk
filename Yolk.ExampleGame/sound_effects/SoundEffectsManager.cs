namespace Yolk.ExampleGame.SoundEffects;

using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.SoundEffects;

public interface ISoundEffectsManager : INode;

[Meta(typeof(IAutoNode))]
public partial class SoundEffectsManager : Node, ISoundEffectsManager {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private ISoundEffectsRepo SoundEffectsRepo => this.DependOn<ISoundEffectsRepo>();

  [Export] private int PoolCount { get; set; } = 10;

  public void OnResolved() {
    CreatePool(PoolCount);

    SoundEffectsRepo.Played += OnSoundEffectPlayed;
  }

  private void OnSoundEffectPlayed(string path) {
    var player = GetAvailablePlayer();
    if (player == null) {
      GD.PrintErr("No available AudioStreamPlayer2D found.");
      return;
    }
    player.Stream = GD.Load<AudioStream>(path);
    player.Play();
    return;
  }

  private void CreatePool(int count) {
    for (var i = 0; i < count; i++) {
      var streamPlayer2D = new AudioStreamPlayer {
        Bus = "SFX"
      };
      AddChild(streamPlayer2D);
    }
  }

  private AudioStreamPlayer? GetAvailablePlayer() => GetChildren()
    .OfType<AudioStreamPlayer>()
    .FirstOrDefault(player => !player.IsPlaying());
}
