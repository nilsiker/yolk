namespace Yolk.ExampleGame.Music;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.Music;

public interface IMusicManager : INode;

[Meta(typeof(IAutoNode))]
public partial class MusicManager : Node, IMusicManager {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IMusicRepo MusicRepo => this.DependOn<IMusicRepo>();

  [Node] private AudioStreamPlayer Track1 { get; set; } = default!;
  [Node] private AudioStreamPlayer Track2 { get; set; } = default!;

  public void OnResolved() {
    MusicRepo.Started += OnMusicStarted;
    MusicRepo.Stopped += OnMusicStopped;

    this.Provide();
  }


  private void OnMusicStopped() {
    FadeOutTrack1(0.0f);
    FadeOutTrack2(0.0f);
  }

  private void OnMusicStarted(string musicPath, float crossfade, float delay) {
    if (IsPlayingTrack(musicPath)) {
      return;
    }

    if (Track1.Playing) {
      FadeOutTrack1(crossfade);
      FadeInTrack2(musicPath, crossfade, delay);
    }
    else if (Track2.Playing) {
      FadeOutTrack2(crossfade);
      FadeInTrack1(musicPath, crossfade, delay);
    }
    else {
      FadeInTrack1(musicPath, crossfade, delay);
    }
  }

  private bool IsPlayingTrack(string musicPath)
    => (Track1.Stream?.ResourcePath == musicPath && Track1.Playing)
      || (Track2.Stream?.ResourcePath == musicPath && Track2.Playing);

  private void FadeInTrack1(string musicPath, float crossfade, float delay) {
    Track1.Stream = GD.Load<AudioStream>(musicPath);
    Track1.Play();
    var tween = Track1.CreateTween();
    tween.SetTrans(Tween.TransitionType.Cubic);
    tween.SetEase(Tween.EaseType.InOut);
    tween.TweenProperty(Track1, "volume_linear", 1.0f, crossfade).SetDelay(delay);
  }

  private void FadeOutTrack1(float crossfade) {
    var tween = Track1.CreateTween();
    tween.SetTrans(Tween.TransitionType.Cubic);
    tween.SetEase(Tween.EaseType.InOut);
    tween.TweenProperty(Track1, "volume_linear", 0.0f, crossfade);
    tween.TweenCallback(Callable.From(Track1.Stop));
  }

  private void FadeInTrack2(string musicPath, float crossfade, float delay) {
    Track2.Stream = GD.Load<AudioStream>(musicPath);
    Track2.Play();
    var tween = Track2.CreateTween();
    tween.SetTrans(Tween.TransitionType.Cubic);
    tween.SetEase(Tween.EaseType.InOut);
    tween.TweenProperty(Track2, "volume_linear", 1.0f, crossfade).SetDelay(delay);
  }

  private void FadeOutTrack2(float crossfade) {
    var tween = Track2.CreateTween();
    tween.SetTrans(Tween.TransitionType.Cubic);
    tween.SetEase(Tween.EaseType.InOut);
    tween.TweenProperty(Track2, "volume_linear", 0.0f, crossfade);
    tween.TweenCallback(Callable.From(Track2.Stop));
  }
}
