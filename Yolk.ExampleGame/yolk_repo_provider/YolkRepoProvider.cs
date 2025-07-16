
namespace Yolk;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.Music;
using Yolk.Logic.SoundEffects;


[Meta(typeof(IAutoNode))]
public partial class YolkRepoProvider : Node,
  IProvide<IInventoryRepo>,
  IProvide<ISoundEffectsRepo>,
  IProvide<IMusicRepo> {
  public override void _Notification(int what) => this.Notify(what);

  IInventoryRepo IProvide<IInventoryRepo>.Value() => InventoryRepo;
  ISoundEffectsRepo IProvide<ISoundEffectsRepo>.Value() => SoundEffectsRepo;
  IMusicRepo IProvide<IMusicRepo>.Value() => MusicRepo;

  private IInventoryRepo InventoryRepo { get; set; } = default!;
  private ISoundEffectsRepo SoundEffectsRepo { get; set; } = default!;
  private IMusicRepo MusicRepo { get; set; } = default!;

  public void OnResolved() {
    InventoryRepo = new InventoryRepo();
    SoundEffectsRepo = new SoundEffectsRepo();
    MusicRepo = new MusicRepo();

    this.Provide();
  }

}
