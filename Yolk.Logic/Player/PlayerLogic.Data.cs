
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {
  [Meta, Id("playerlogic_data")]
  public partial class Data {
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
  }
}
