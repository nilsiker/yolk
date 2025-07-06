namespace Yolk.Game;

using Yolk.Data;


public partial class GameLogic {
  public static class Input {
    public record struct Ready;
    public record struct OnPauseUserInput;
    public record struct OnQuitRequested;
    public record struct Start();
    public record struct OnGameOverTriggered;
    public record struct Save(string? SaveName, ESaveType SaveType);
    public record struct Load(string? SaveName, ESaveType SaveType);
    public record struct BlackoutFinished;
    public record struct OnSaved;
    public record struct OnLoaded;
    public record struct QuittingTransitionFinished;
  }
}
