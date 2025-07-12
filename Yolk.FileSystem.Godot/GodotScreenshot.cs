namespace Yolk.FS;

using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public static class GodotScreenshot {
  public static async void Save(string filePath, List<uint> hideLayers) => await SaveAsync(filePath, hideLayers);

  public static async Task SaveAsync(string filePath, List<uint> hideLayers) {
    var tree = Engine.GetMainLoop() as SceneTree;
    if (tree?.CurrentScene == null) {
      GD.PrintErr("Cannot take screenshot: No current scene");
      return;
    }

    // TODO revert to tree if dropping viewport idea
    var mainViewport = tree.CurrentScene.GetNode("%Game").GetViewport();

    // Hide layers on main thread
    foreach (var layer in hideLayers) {
      mainViewport.SetCanvasCullMaskBit(layer, false);
    }

    await tree.ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);

    // Capture the image - this must happen on main thread
    var image = mainViewport.GetTexture().GetImage();

    // Restore layers on main thread
    foreach (var layer in hideLayers) {
      mainViewport.SetCanvasCullMaskBit(layer, true);
    }

    // Save the image asynchronously on a background thread
    await Task.Run(() => {
      var error = image.SavePng(filePath);

      if (error != Error.Ok) {
        // Use CallDeferred to safely call GD.PrintErr from background thread
        Callable.From(() => GD.PrintErr($"Failed to save screenshot to {filePath}: {error}")).CallDeferred();
      }
      else {
        Callable.From(() => GD.Print($"Screenshot saved to {filePath}")).CallDeferred();
      }
    });
  }
}
