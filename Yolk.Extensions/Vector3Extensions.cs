
namespace Yolk.Extensions;

using Godot;

public static class Vector3Extensions {
  public static Vector3 RadToDeg(this Vector3 vector) => vector with {
    X = Mathf.RadToDeg(vector.X),
    Y = Mathf.RadToDeg(vector.Y),
    Z = Mathf.RadToDeg(vector.Z),
  };
}
