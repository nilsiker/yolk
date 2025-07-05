
namespace Yolk.Logic.Controls;

using Godot;

public partial class ControlsLogic {

  public static class Input {
    public readonly record struct Listen(string Action, EActionType ActionType);
    public readonly record struct PressKey(InputEventKey Key);
    public readonly record struct Cancel;
  }
}

