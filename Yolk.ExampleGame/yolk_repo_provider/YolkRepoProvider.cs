
namespace Yolk;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

using Yolk.ExampleGame;
using Yolk.Logic.Music;
using Yolk.Logic.Player;
using Yolk.Logic.SoundEffects;


[Meta(typeof(IAutoNode))]
public partial class YolkRepoProvider : Node,
  IProvide<ISoundEffectsRepo>,
  IProvide<IMusicRepo>,
  IProvide<IPlayerRepo> {
  public override void _Notification(int what) => this.Notify(what);

  [Node] private PlayerProvider PlayerProvider { get; set; } = default!;

  ISoundEffectsRepo IProvide<ISoundEffectsRepo>.Value() => SoundEffectsRepo;
  IMusicRepo IProvide<IMusicRepo>.Value() => MusicRepo;
  IPlayerRepo IProvide<IPlayerRepo>.Value() => PlayerProvider.PlayerRepo;

  private ISoundEffectsRepo SoundEffectsRepo { get; set; } = default!;
  private IMusicRepo MusicRepo { get; set; } = default!;

  public void OnResolved() {
    SoundEffectsRepo = new SoundEffectsRepo();
    MusicRepo = new MusicRepo();

    this.Provide();
  }

}
