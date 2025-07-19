
namespace Yolk.ExampleGame.UI.HUD;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Tool]
[Meta(typeof(IAutoNode))]
public partial class PipUI : TextureRect {
  public override void _Notification(int what) => this.Notify(what);

  private bool _filled = true;

  [Export]
  public bool Filled {
    get => _filled; set {

      _filled = value;
      if (IsInsideTree()) {
        if (_filled) {
          Texture = _texture;
          Modulate = _color;
        }
        else {
          Texture = _emptyTexture;
          Modulate = _emptyColor;
        }
      }
    }
  }

  [Export] private Texture2D _texture = default!;
  [Export] private Texture2D _emptyTexture = default!;
  [Export] private Color _color = new(1, 1, 1, 1);
  [Export] private Color _emptyColor = new(0.5f, 0.5f, 0.5f, 1);

  public void Setup() {
    Texture = _texture;
    Modulate = _color;
  }

  public void OnResolved() {

  }
}
