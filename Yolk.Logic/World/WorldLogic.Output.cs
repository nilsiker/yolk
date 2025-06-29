namespace Yolk.Level;

public partial class WorldLogic {
  public static class Output {
    public record struct LoadLevel(string LevelName);
    public record struct LevelLoaded(string PreviousLevel);
  }
}


