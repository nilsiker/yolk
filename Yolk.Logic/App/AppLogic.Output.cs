namespace Yolk;

public partial class AppLogic {
  public static class Output {
    public record struct QuitApp;
    public record struct SetMouseCaptured(bool Captured);
    public record struct SetBlackout(bool On);
  }
}
