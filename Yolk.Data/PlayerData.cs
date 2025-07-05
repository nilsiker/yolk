
namespace Yolk.Data;

using Chickensoft.Introspection;
using Chickensoft.Serialization;

[Meta, Id("player_data")]
public partial record PlayerData {
  [Save("px")]
  public float Px { get; init; }
  [Save("py")]
  public float Py { get; init; }
  [Save("pz")]
  public float Pz { get; init; }
}
