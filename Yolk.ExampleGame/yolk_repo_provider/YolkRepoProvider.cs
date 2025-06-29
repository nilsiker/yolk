
namespace Yolk;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode))]
public partial class YolkRepoProvider : Node,
  IProvide<IInventoryRepo>,
  IProvide<ISoundRepo> {
  public override void _Notification(int what) => this.Notify(what);

  private IInventoryRepo InventoryRepo { get; set; } = default!;
  private ISoundRepo SoundRepo { get; set; } = default!;

  IInventoryRepo IProvide<IInventoryRepo>.Value() => InventoryRepo;
  ISoundRepo IProvide<ISoundRepo>.Value() => SoundRepo;

  public void OnResolved() {
    InventoryRepo = new InventoryRepo();
    SoundRepo = new SoundRepo();

    this.Provide();
  }
}
