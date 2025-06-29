namespace Yolk;

using Godot;

public partial class AppLogic {
  public static class Input {
    public record struct RequestGameStart;
    public record struct RequestGameLoad;
    public record struct RequestAppQuit;
    public record struct ReleasedMouseMotionOccurred(Vector2 NewPosition);
    public record struct BlackoutRequested(BlackoutCallback Callback);
    public record struct BlackoutFinished;
  }
}
