namespace Yolk.Data;

using Chickensoft.Introspection;
using Chickensoft.Serialization;

[Meta, Id("world_data")]
public partial record WorldData {
  [Save("current_level_name")]
  public required string CurrentLevelName { get; init; }
}
