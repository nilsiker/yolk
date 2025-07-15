namespace Yolk.Extensions;

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
      child.Name += "_FREEING";
      child.QueueFree();
    }
  }

  public static void Disable(this Node node, bool visible = true) {
    node.ProcessMode = Node.ProcessModeEnum.Disabled;

    if (node is Node2D node2D) {
      node2D.Visible = !visible;
    }
    else if (node is Node3D node3D) {
      node3D.Visible = !visible;
    }
    else if (node is Control control) {
      control.Visible = !visible;
    }
  }

  public static void Enable(this Node node, bool visible = true) {
    node.ProcessMode = Node.ProcessModeEnum.Inherit;

    if (node is Node2D node2D) {
      node2D.Visible = visible;
    }
    else if (node is Node3D node3D) {
      node3D.Visible = visible;
    }
    else if (node is Control control) {
      control.Visible = visible;
    }
  }
}
