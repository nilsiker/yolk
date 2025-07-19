namespace Yolk.Core;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.ExampleGame;
using Yolk.Game;

[Meta(typeof(IAutoNode))]
public partial class AutosavePoint : Area2D {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  public void OnResolved() => BodyEntered += (body) => {
    if (body is IPlayer player) {
      GameRepo.Autosave();
      player.RegisterCheckpoint(GlobalPosition.X, GlobalPosition.Y);
    }
  };
}
