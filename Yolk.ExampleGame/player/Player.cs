namespace Yolk.ExampleGame;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Godot;

public interface IPlayer : ICharacterBody2D;

[Meta(typeof(IAutoNode))]
public partial class Player : CharacterBody2D, IPlayer {
  public override void _Notification(int what) => this.Notify(what);

}
