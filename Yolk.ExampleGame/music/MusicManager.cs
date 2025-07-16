namespace Yolk.ExampleGame.Music;

using System;
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

  public void OnResolved() {

    MusicRepo.Started += OnMusicStarted;
    MusicRepo.Stopped += OnMusicStopped;


    this.Provide();
  }


  private void OnMusicStopped() => throw new NotImplementedException();
  private void OnMusicStarted(string musicName) => throw new NotImplementedException();

  private void CreatePool(int count) {
    for (var i = 0; i < count; i++) {
      var player = new AudioStreamPlayer();
      AddChild(player);
    }
  }
}
