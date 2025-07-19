
namespace Yolk.Logic.Player;

using Chickensoft.Introspection;

public partial class PlayerLogic {
  [Meta, Id("playerlogic_data")]
  public partial class Data {
    public float CheckpointX { get; set; }
    public float CheckpointY { get; set; }
    public float Speed { get; set; } = 75.0f;
    public float MoveDirectionX { get; set; }
    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
  }
}
