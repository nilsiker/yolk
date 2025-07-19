
namespace Yolk.Logic.Player;

public partial class PlayerLogic {
  public static class Input {
    public readonly record struct TakeDamage(int Amount);
    public readonly record struct Die;
    public readonly record struct AnimationFinished;
    public readonly record struct BlackoutFinished;
  }
}
