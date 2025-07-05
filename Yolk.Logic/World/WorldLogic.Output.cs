namespace Yolk.Level;

public partial class WorldLogic {
  public static class Output {
    public record struct TransitionLevel(string LevelName, string? FromLevelName = null);
  }
}


