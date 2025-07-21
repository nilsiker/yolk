namespace Yolk.ExampleGame.UI.HUD;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Extensions;
using Yolk.Logic.Player;

// TODO fix heart and charge pips to reflect actual health and charge values

[Meta(typeof(IAutoNode))]
public partial class Gauges : Control {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private PackedScene _heartScene = default!;
  [Export] private PackedScene _chargeScene = default!;

  [Dependency] private IPlayerRepo PlayerRepo => this.DependOn<IPlayerRepo>();

  [Node] private Control Hearts { get; set; } = default!;
  [Node] private Control Charges { get; set; } = default!;

  public void OnResolved() {
    PlayerRepo.Health.Sync += OnPlayerHeartsSync;
    PlayerRepo.Charges.Sync += OnPlayerChargesSync;
    PlayerRepo.MaxHealth.Sync += OnPlayerMaxHealthSync;
    PlayerRepo.MaxCharges.Sync += OnPlayerMaxChargesSync;
  }

  private void OnPlayerMaxHealthSync(int count) {
    Hearts.ClearChildren();

    for (var i = 0; i < count; i++) {
      var pip = _heartScene.Instantiate<PipUI>();
      Hearts.AddChild(pip);
    }

    FillHearts(count);
  }

  private void OnPlayerMaxChargesSync(int count) {
    Charges.ClearChildren();

    for (var i = 0; i < count; i++) {
      var pip = _chargeScene.Instantiate<PipUI>();
      Charges.AddChild(pip);
    }

    FillCharges(count);
  }

  public void FillHearts(int count) {
    foreach (var child in Hearts.GetChildren()) {
      if (child.IsQueuedForDeletion()) {
        continue;
      }


      if (child is PipUI pip) {
        pip.Filled = count > 0;
        count--;
      }
    }
  }

  public void FillCharges(int count) {
    foreach (var child in Charges.GetChildren()) {
      if (child is PipUI pip) {
        if (child.IsQueuedForDeletion()) {
          continue;
        }

        pip.Filled = count > 0;
        count--;
      }
    }
  }


  private void OnPlayerHeartsSync(int hearts) => Callable.From(() => FillHearts(hearts)).CallDeferred();

  private void OnPlayerChargesSync(int charges) => Callable.From(() => FillCharges(charges)).CallDeferred();
}
