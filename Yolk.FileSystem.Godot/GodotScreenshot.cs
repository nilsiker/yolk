namespace Yolk.Game;

using System.Collections.Generic;
using Godot;

public static class GodotScreenshot {
  public static async void Save(string filePath, List<uint> hideLayers) {
    var tree = Engine.GetMainLoop() as SceneTree;
    if (tree?.CurrentScene == null) {
      GD.PrintErr("Cannot take screenshot: No current scene");
      return;
    }

    var mainViewport = tree.Root;

    foreach (var layer in hideLayers) {
      mainViewport.SetCanvasCullMaskBit(layer, false);
    }

    var image = mainViewport.GetTexture().GetImage();
    await tree.ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);

    var error = image.SavePng(filePath);

    if (error != Error.Ok) {
      GD.PrintErr($"Failed to save screenshot to {filePath}: {error}");
    }
    else {
      GD.Print($"Screenshot saved to {filePath}");
    }

    foreach (var layer in hideLayers) {
      mainViewport.SetCanvasCullMaskBit(layer, true);
    }
  }
}
