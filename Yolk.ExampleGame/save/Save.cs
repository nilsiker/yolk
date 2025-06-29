namespace Yolk;

using System;
using System.IO.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;
using Chickensoft.Collections;
using Chickensoft.Serialization;
using Godot;
using Yolk.Data;

public class Save {
  private static readonly IReadOnlyBlackboard _upgradeDependencies = new Blackboard();
  private static readonly SerializableTypeResolver _resolver = new();
  public static JsonSerializerOptions JSON_OPTIONS => new() {
    Converters = {
        new SerializableTypeConverter(_upgradeDependencies)
    },
    TypeInfoResolver = _resolver,
    WriteIndented = true
  };

  public static async Task SaveGame(GameData data, int slot) {
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

  public static async Task<GameData?> LoadGame(int slot) {
    GD.Print("loading from file...");

    var fileSystem = new FileSystem();
    var path = fileSystem.Path.Join(OS.GetUserDataDir(), $"save_{slot}.json");

    if (!fileSystem.File.Exists(path)) {
      GD.Print("No save file to load :'(");
      return null;
    }

    var json = await fileSystem.File.ReadAllTextAsync(path);

    return JsonSerializer.Deserialize<GameData>(json, JSON_OPTIONS);
  }
}
