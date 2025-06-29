namespace Yolk;

public partial class PauseMenuLogic {
  public static class Input {
    public record struct Show;
    public record struct Hide;
    public record struct OnResumePressed;
    public record struct OnOptionsPressed;
    public record struct OnQuitPressed(bool QuitToDesktop);
  }
}
