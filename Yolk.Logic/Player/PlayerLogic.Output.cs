
namespace Yolk.Logic.Player;


public partial class PlayerLogic {
  public static class Output {
    public readonly record struct Teleport(float X, float Y);
    public readonly record struct Died;
    public readonly record struct SetEnabled(bool Enabled);
    public readonly record struct Animate(string Animation);
    public readonly record struct MoveAndSlide(float X, float Y);
    public readonly record struct FaceRight;
    public readonly record struct FaceLeft;
    public readonly record struct OnJump;
    public readonly record struct GrantInvincibility(float Duration);
  }
}
