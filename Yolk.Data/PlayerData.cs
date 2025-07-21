
namespace Yolk.Data;

using Chickensoft.Introspection;
using Chickensoft.Serialization;
using Yolk.Logic.Player;


[Meta, Id("player_data")]
public partial record PlayerData {
  [Save("playerlogic")]
  public required PlayerLogic Logic { get; init; }
  [Save("px")]
  public float Px { get; init; }
  [Save("py")]
  public float Py { get; init; }
  [Save("pz")]
  public float Pz { get; init; }
  [Save("health")]
  public int Health { get; init; }
  [Save("max_health")]
  public int MaxHealth { get; init; }
  [Save("charges")]
  public int Charges { get; init; }
  [Save("max_charges")]
  public int MaxCharges { get; init; }
}
