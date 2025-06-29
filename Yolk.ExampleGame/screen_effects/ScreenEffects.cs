namespace Yolk;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;

[Meta(typeof(IAutoNode))]
public partial class ScreenEffects : ColorRect {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private IOptionsRepo Options => this.DependOn<IOptionsRepo>();

  private ShaderMaterial ShaderMaterial => (ShaderMaterial)Material;
  private string _quantizeSize = "quantize_size";
  private string _dithering = "dither_enabled";

  public bool Pixelation {
    set => Visible = value;
  }

  public bool Dithering {
    set => ShaderMaterial.SetShaderParameter(_dithering, value);
  }

  public void OnResolved() {
    Options.Pixelation.Sync += OnOptionsPixelationSync;
    Options.Dithering.Sync += OnOptionsDitheringSync;
  }

  private void OnOptionsPixelationSync(bool enabled) => Pixelation = enabled;
  private void OnOptionsDitheringSync(bool enabled) => Dithering = enabled;

}
