
namespace Yolk.Logic.Controls;

using Godot;

public partial class ControlsLogic {

  public static class Output {
    public readonly record struct ActionMapped(string Action, InputEventKey Key);
  }
}

