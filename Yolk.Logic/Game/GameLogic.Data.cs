namespace Yolk.Game;

public enum ELoadType {
  Manual,
  Auto,
  Quick
}

public partial class GameLogic {
  public class Data() {
    public required string SaveName;

    public required ELoadType LoadType;
  }
}
