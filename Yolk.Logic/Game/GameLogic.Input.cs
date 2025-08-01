namespace Yolk.Game;

public partial class GameLogic {
  public static class Input {
    public record struct Ready;
    public record struct OnPauseUserInput;
    public record struct OnQuitRequested;
    public record struct Start();
    public record struct OnGameOverTriggered;
    public record struct Save(string SaveName);
    public record struct Load(string SaveName);
    public record struct DeleteSave(string SaveName);
    public record struct Autosave;
    public record struct Autoload;
    public record struct Quicksave;
    public record struct Quickload;
    public record struct BlackoutFinished;
    public record struct OnSaved(string SaveName);
    public record struct OnDeleted(string SaveName);
    public record struct OnLoaded;
    public record struct QuittingTransitionFinished;
  }
}
