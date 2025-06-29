namespace Yolk.Game;

public partial class GameLogic {
  public static class Input {
    public record struct OnPauseUserInput;
    public record struct OnQuitRequested;
    public record struct OnStartRequested(int Slot);
    public record struct OnGameOverTriggered;
    public record struct Save(int Slot);
    public record struct Load(int Slot);
    public record struct BlackoutFinished;
    public record struct OnLoaded;
    public record struct QuittingTransitionFinished;
  }
}
