
namespace Yolk.Data;

using Chickensoft.Introspection;
using Chickensoft.Serialization;
using Yolk.Logic.Player;


[Meta, Id("player_data")]
public partial record PlayerData {
  [Save("px")]
  public float Px { get; init; }
  [Save("py")]
  public float Py { get; init; }
  [Save("pz")]
  public float Pz { get; init; }
  [Save("playerlogic")]
  public required PlayerLogic Logic { get; init; }
}
