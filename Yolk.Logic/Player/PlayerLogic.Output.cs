
namespace Yolk.Logic.Player;

using Yolk.Logic.World;

public partial class PlayerLogic {
  public static class Output {
    public readonly record struct Teleport(Transform Entrypoint);
    public readonly record struct Died;
  }
}
