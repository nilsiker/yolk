namespace Yolk;

using System.Diagnostics.CodeAnalysis;
using Godot;

public static class NodeExtensions {
  public static void ResetTween(this Node node, [NotNull] ref Tween? tween) {
    if (tween is not null && tween.IsRunning()) {
      tween.Kill();
    }

    tween = node.CreateTween();
  }

  public static void ClearChildren(this Node node) {
    foreach (var child in node.GetChildren()) {
      child.QueueFree();
    }
  }
}
