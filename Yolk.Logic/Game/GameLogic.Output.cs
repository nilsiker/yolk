namespace Yolk.Game;

public partial class GameLogic {
  public static class Output {
    public record struct SetPauseMode(bool Paused);
    public record struct UpdateVisibility(bool Visible);
    public record struct SetBlackout(bool On);
    public record struct QuitGame;
    public record struct SaveGame(string SaveName);
    public record struct LoadGame(string SaveName);
    public record struct DeleteSave(string SaveName);
    public record struct Autosave;
    public record struct Autoload;
    public record struct Quicksave;
    public record struct Quickload;
    public record struct GameOver;
  }
}
