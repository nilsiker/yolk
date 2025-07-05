namespace Yolk.Logic.World;

using Godot;

public class Vec2 {
  public float X { get; set; }
  public float Y { get; set; }
}

public class Vec3 {
  public float X { get; set; }
  public float Y { get; set; }
  public float Z { get; set; }
}

public interface ITransform2D {
  public Vec2 Position { get; }
  public float Rotation { get; }
}


public interface ITransform3D {
  public Vec3 Position { get; }
  public Vec3 Rotation { get; }
}

public class Transform : ITransform3D, ITransform2D {
  private readonly float _posX;
  private readonly float _poxY;
  private readonly float _posZ;

  private readonly float _rotX;
  private readonly float _rotY;
  private readonly float _rotZ;

  public Vec3 Position => new() { X = _posX, Y = _poxY, Z = _posZ };
  public Vec3 Rotation => new() { X = _rotX, Y = _rotY, Z = _rotZ };

  Vec2 ITransform2D.Position => new() { X = _posX, Y = _poxY };
  float ITransform2D.Rotation => Rotation.Z;

  public Transform(Vec2 vector, float rotation = 0.0f) {
    _posX = vector.X;
    _poxY = vector.Y;
    _rotZ = rotation;
  }
  public Transform(Vec3 vector, Vec3 eulerAngles) {
    _posX = vector.X;
    _poxY = vector.Y;
    _posZ = vector.Z;

    _rotX = eulerAngles.X;
    _rotY = eulerAngles.Y;
    _rotZ = eulerAngles.Z;
  }
}

public static class GodotExtensions {
  public static Vec3 Vec3(this Vector3 vector) => new() {
    X = vector.X,
    Y = vector.Y,
    Z = vector.Z
  };

  public static Vec2 Vec2(this Vector2 vector) => new() {
    X = vector.X,
    Y = vector.Y,
  };
}
