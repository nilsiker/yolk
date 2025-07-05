namespace Yolk.Level;

using Yolk.Logic.World;

public partial class WorldLogic {
  public static class Input {
    public record struct Transition(string ToLevelName, string? FromLevelName = null);
    public record struct LoadLevel(string LevelName);
    public record struct UnloadLevel(string LevelName);
    public record struct OnTransitioned(Transform? Entrypoint = null);
    public record struct Exit;
  }
}
