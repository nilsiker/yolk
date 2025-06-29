namespace Yolk.Level;

using Godot;

public partial class WorldLogic {
  public static class Input {
    public record struct RequestLevelTransition(string FromLevelName, string ToLevelName, bool SkipTransition = false);
    public record struct OnLevelLoaded(Transform3D? LandingTransform = null);
  }
}


