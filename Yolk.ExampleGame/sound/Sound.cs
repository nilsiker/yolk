namespace Yolk;

using System;
using Godot;

public partial class Sound : AudioStreamPlayer3D {
  private Area3D _area = default!;
  private CollisionShape3D _coll = default!;
  public SoundResource? SoundResource { get; set; }

  public override void _Ready() {
    if (SoundResource is null) {
      throw new MissingMemberException();
    }

    Finished += OnFinished;

    _area = GetNode<Area3D>("Area3D");
    _coll = GetNode<CollisionShape3D>("Area3D/CollisionShape3D");

    var sphere = (SphereShape3D)_coll.Shape;

    Stream = SoundResource.Stream;
    sphere.Radius = SoundResource?.Distance ?? throw new MissingFieldException();

    Play();
  }

  private void OnFinished() {
    foreach (var body in _area.GetOverlappingBodies()) {
      if (body is ISoundNoticer noticer) {
        noticer.NoticeSound(GlobalPosition);
      }
    }

    QueueFree();
  }
}
