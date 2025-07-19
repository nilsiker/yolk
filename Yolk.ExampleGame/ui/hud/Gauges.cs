namespace Yolk.ExampleGame.UI.HUD;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Extensions;
using Yolk.Logic.Player;


[Meta(typeof(IAutoNode))]
public partial class Gauges : Control {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private PackedScene _heartScene = default!;
  [Export] private PackedScene _chargeScene = default!;

  [Dependency] private IPlayerRepo PlayerRepo => this.DependOn<IPlayerRepo>();

  [Node] private Control Hearts { get; set; } = default!;
  [Node] private Control Charges { get; set; } = default!;

  public void OnResolved() {
    PlayerRepo.Hearts.Sync += OnPlayerHeartsSync;
    PlayerRepo.Charges.Sync += OnPlayerChargesSync;


    Hearts.ClearChildren();
    Charges.ClearChildren();

    for (var i = 0; i < PlayerRepo.Hearts.Value; i++) {
      var pip = _heartScene.Instantiate<PipUI>();
      pip.Filled = true;
      Hearts.AddChild(pip);
    }

    for (var i = 0; i < PlayerRepo.Charges.Value; i++) {
      var pip = _chargeScene.Instantiate<PipUI>();
      pip.Filled = true;
      Charges.AddChild(pip);
    }
  }

  private void OnPlayerHeartsSync(int hearts) {
    foreach (var child in Hearts.GetChildren()) {
      if (child is PipUI pip) {
        pip.Filled = hearts > 0;
        hearts--;
      }
    }
  }

  private void OnPlayerChargesSync(int charges) {
    foreach (var child in Charges.GetChildren()) {
      if (child is PipUI pip) {
        pip.Filled = charges > 0;
        charges--;
      }
    }
  }
}
