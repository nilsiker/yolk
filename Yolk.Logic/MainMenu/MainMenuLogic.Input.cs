namespace Yolk;

public partial class MainMenuLogic {
  public static class Input {
    public record struct OnStartGamePressed;
    public record struct OnQuitButtonPressed;
    public record struct Hide;
    public record struct Show;
  }
}
