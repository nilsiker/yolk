namespace Yolk.Game;

public partial class GameLogic {
  public static class Output {
    public record struct SetPauseMode(bool Paused);
    public record struct UpdateVisibility(bool Visible);
    public record struct SetBlackout(bool On);
    public record struct QuitGame;
    public record struct SaveGame(int Slot);
    public record struct LoadGame(int Slot);
    public record struct SetSlot(int Slot);
    public record struct GameOver;
  }
}
