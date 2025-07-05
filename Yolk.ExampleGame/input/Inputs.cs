namespace Yolk.Controls;

using Godot;

[InputMap]
public static partial class Inputs {
  public static Vector2 GetMoveVector() => Input.GetVector(Left, Right, Forward, Backward);
}
