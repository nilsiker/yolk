namespace Yolk.Data;

using Chickensoft.Introspection;
using Chickensoft.Serialization;

[Meta, Id("game_data")]
public partial record GameData {
  [Save("world_data")]
  public required WorldData WorldData { get; init; }
}
