
namespace Yolk.FS;

using System.IO.Abstractions;
using System.Runtime.Serialization;
using System.Text.Json;
using Chickensoft.Collections;
using Chickensoft.Serialization;
using Chickensoft.Serialization.Godot;
using Godot;

public static class GodotSave {
  private static readonly IReadOnlyBlackboard _upgradeDependencies = new Blackboard();
  private static readonly SerializableTypeResolver _resolver = new();
  public static JsonSerializerOptions JSON_OPTIONS => new() {
    Converters = {
        new SerializableTypeConverter(_upgradeDependencies)
    },
    TypeInfoResolver = _resolver,
    WriteIndented = true
  };

  public static void Setup() => GodotSerialization.Setup();

  public static async Task Save<T>(T data, int slot) {
    GD.Print("saving to file...");

    var fileSystem = new FileSystem();

    var path = fileSystem.Path.Join(OS.GetUserDataDir(), $"save_{slot}.json");

    try {
      var json = JsonSerializer.Serialize(data, JSON_OPTIONS);
      await fileSystem.File.WriteAllTextAsync(path, json);
    }
    catch (Exception e) {
      GD.PushError($"Failed to save game: {e}");
    }
  }

  public static async Task<T> Load<T>(int slot) {
    GD.Print("loading from file...");

    var fileSystem = new FileSystem();
    var path = fileSystem.Path.Join(OS.GetUserDataDir(), $"save_{slot}.json");

    if (!fileSystem.File.Exists(path)) {
      GD.Print("No save file to load :'(");
      throw new SerializationException("could not find save file");
    }

    var json = await fileSystem.File.ReadAllTextAsync(path);

    return JsonSerializer.Deserialize<T>(json, JSON_OPTIONS) ?? throw new SerializationException("could not serialize save game data");
  }
}
