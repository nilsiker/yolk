namespace Yolk;

using System;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface ISoundSpawner : INode { }

[Meta(typeof(IAutoNode))]
public partial class SoundSpawner : Node, ISoundSpawner {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private PackedScene? _soundScene;

  #region Dependencies
  [Dependency] private ISoundRepo SoundRepo => this.DependOn<ISoundRepo>();
  #endregion

  #region Dependency Lifecycle
  public void OnResolved() => SoundRepo.Sounded += OnSoundSounded;

  private void OnSoundSounded(ISound sound, Vector3 atPosition) {
    var soundNode = _soundScene?.Instantiate<Sound>() ?? throw new MissingFieldException();
    soundNode.SoundResource = sound as SoundResource;

    AddChild(soundNode);
    soundNode.GlobalPosition = atPosition;
  }
  #endregion
}
