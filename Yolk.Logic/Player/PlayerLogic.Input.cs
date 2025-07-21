
namespace Yolk.Logic.Player;

public partial class PlayerLogic {
  public static class Input {
    public readonly record struct OnGameStarting;
    public readonly record struct PhysicsTick(float Delta);
    public readonly record struct TakeDamage(int Amount);
    public readonly record struct Die;
    public readonly record struct AnimationFinished;
    public readonly record struct BlackoutFinished;
    public readonly record struct Move(float X, float Y);
    public readonly record struct Jump;
    public readonly record struct StopJump;
    public readonly record struct Dash;
    public readonly record struct ClingToWall;
    public readonly record struct OnGrounded(bool IsGrounded);
    public readonly record struct HitCeiling;
    public readonly record struct RegisterCheckpoint(float X, float Y);
  }
}
