namespace Yolk.ExampleGame;

using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Logic.Player;

[Meta(typeof(IAutoNode))]
public partial class PlayerCamera : Camera2D {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IPlayerRepo PlayerRepo => this.DependOn<IPlayerRepo>();

  private float _trauma;
  private float _traumaPower = 2.0f;
  private float _decayTime = 0.5f;
  private Vector2 _maxOffset = new(16, 16);

  private Tween? _shakeTween;

  public void OnResolved() {
    PlayerRepo.Damaged += OnPlayerDamaged;
    PlayerRepo.Healed += OnPlayerHealed;
  }

  public override void _Process(double delta) {
    if (_trauma > 0f) {
      var decayRate = (float)(0.5 / _decayTime);
      _trauma = Math.Max(_trauma - (decayRate * (float)delta), 0f);
      ApplyShake();
    }
    else if (Offset != Vector2.Zero && _shakeTween == null) {
      // Smoothly return to center if not already tweening
      _shakeTween = CreateTween();
      _shakeTween.TweenProperty(this, "offset", Vector2.Zero, 0.1f)
        .SetTrans(Tween.TransitionType.Sine)
        .SetEase(Tween.EaseType.Out)
        .Finished += () => _shakeTween = null;
    }
  }

  private void ApplyShake() {
    var amount = MathF.Pow(_trauma, _traumaPower);
    var x = _maxOffset.X * amount * (float)GD.RandRange(-1f, 1f);
    var y = _maxOffset.Y * amount * (float)GD.RandRange(-1f, 1f);
    Offset = new Vector2(x, y);
  }

  private void OnPlayerDamaged() {
    _trauma = Math.Min(_trauma + 0.4f, 1.0f);
    // Cancel any return-to-center tween if shaking again
    _shakeTween?.Kill();
    _shakeTween = null;
  }

  private Tween _zoomTween = default!;
  private void OnPlayerHealed() {
    _zoomTween = CreateTween();
    _zoomTween.SetTrans(Tween.TransitionType.Cubic);
    _zoomTween.TweenProperty(this, "zoom", new Vector2(3.8f, 3.8f), 0.25f);
    _zoomTween.TweenProperty(this, "zoom", new Vector2(4, 4), 0.25f);
  }

  public override void _ExitTree() {
    PlayerRepo.Damaged -= OnPlayerDamaged;
    PlayerRepo.Healed -= OnPlayerHealed;

    base._ExitTree();
  }
}
