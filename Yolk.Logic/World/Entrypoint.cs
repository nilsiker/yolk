namespace Yolk.Logic.World;

using Godot;

public class Entrypoint {
  public float PositionX { get; set; }
  public float PositionY { get; set; }
  public float PositionZ { get; set; }

  public float RotationX { get; set; }
  public float RotationY { get; set; }
  public float RotationZ { get; set; }

  public Entrypoint(Vector2 vector, float rotation = 0.0f) {
    PositionX = vector.X;
    PositionY = vector.Y;
    RotationZ = rotation;
  }
  public Entrypoint(Vector3 vector, Vector3 eulerAngles = default) {
    PositionX = vector.X;
    PositionY = vector.Y;
    PositionZ = vector.Z;

    RotationX = eulerAngles.X;
    RotationY = eulerAngles.Y;
    RotationZ = eulerAngles.Z;
  }
}
