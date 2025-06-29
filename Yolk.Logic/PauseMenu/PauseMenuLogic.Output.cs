namespace Yolk;

public partial class PauseMenuLogic {
  public static class Output {
    public record struct UpdateVisibility(bool Visible);
    public record struct QuitGame(bool QuitToDesktop);
  }
}
