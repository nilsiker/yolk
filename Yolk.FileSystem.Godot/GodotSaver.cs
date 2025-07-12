
namespace Yolk.FS;

using System.Data;
using System.IO.Abstractions;
using System.Runtime.Serialization;
using System.Text.Json;
using Chickensoft.Collections;
using Chickensoft.Serialization;
using Chickensoft.Serialization.Godot;
using Godot;

public static class GodotSaver {
  private static readonly FileSystem _fs = new();
  private static readonly IReadOnlyBlackboard _upgradeDependencies = new Blackboard();
  private static readonly SerializableTypeResolver _resolver = new();
  public static JsonSerializerOptions JSON_OPTIONS => new() {
    Converters = {
      new SerializableTypeConverter(_upgradeDependencies)
  },
    TypeInfoResolver = _resolver,
    WriteIndented = true
  };

  public static string SaveDirectory => _fs.Path.Join(OS.GetUserDataDir(), "saves");
  public static string SaveThumbnailDirectory => _fs.Path.Join(OS.GetUserDataDir(), "saves", "thumbnails");

  public static void Initialize() {
    GodotSerialization.Setup();
    _fs.Directory.CreateDirectory(SaveDirectory);
    _fs.Directory.CreateDirectory(SaveThumbnailDirectory);
  }

  public static string GetSaveFilePath(string? saveName) => _fs.Path.Join(OS.GetUserDataDir(), "saves", $"{saveName}.json");

  public static async Task Save<T>(T data, string? saveName) {
    GD.Print($"saving to file... (name: {saveName})");

    var screenshotTask = GodotScreenshot.SaveAsync($"{SaveThumbnailDirectory}/{saveName}.png", [1]);

    var path = GetSaveFilePath(saveName);

    try {
      var json = await Task.Run(() => JsonSerializer.Serialize(data, JSON_OPTIONS));

      await Task.WhenAll(
        screenshotTask,
        _fs.File.WriteAllTextAsync(path, json)
      );
    }
    catch (Exception e) {
      GD.PushError($"Failed to save game: {e}");
    }
  }

  public static async Task<T> Load<T>(string? saveName) {
    GD.Print($"loading from file... (name: {saveName})");

    if (!Exists(saveName)) {
      GD.Print("No save file to load :'(");
      throw new FileNotFoundException("could not find save file");
    }

    var path = GetSaveFilePath(saveName);
    var json = await _fs.File.ReadAllTextAsync(path);
    return JsonSerializer.Deserialize<T>(json, JSON_OPTIONS) ?? throw new SerializationException("could not serialize save game data");
  }

  public static void Delete(string saveName) {
    GD.Print($"deleting save file... (name: {saveName})");

    if (!Exists(saveName)) {
      GD.Print("No save file to delete :'(");
      return;
    }


    var path = GetSaveFilePath(saveName);

    try {
      _fs.File.Delete(path);
      GD.Print($"Deleted save file: {path}");
    }
    catch (Exception e) {
      GD.PushError($"Failed to delete save file: {e}");
    }
  }

  public static bool Exists(string? saveName) => _fs.File.Exists(GetSaveFilePath(saveName));

  public static Texture2D GetThumbnail(string? saveName) {
    var imagePath = _fs.Path.Join(SaveThumbnailDirectory, $"{saveName}.png");

    var image = new Image();
    if (image.Load(imagePath) != Error.Ok) {
      GD.Print("Failed to load preview image.");
      return new PlaceholderTexture2D();
    }

    var texture = ImageTexture.CreateFromImage(image);
    return texture;
  }

  public static Error SaveThumbnail(string? saveName, Image image) {
    var path = GetSaveFilePath(saveName);
    var imagePath = _fs.Path.Join(SaveThumbnailDirectory, $"{saveName}.png");

    return image.SavePng(imagePath);
  }

  public static IEnumerable<GodotSave<T>> GetAllSaveInfo<T>() {
    var savesDir = _fs.Path.Join(OS.GetUserDataDir(), "saves");
    if (!_fs.Directory.Exists(savesDir)) {
      GD.Print("No save directory found.");
      return [];
    }

    var files = _fs.Directory.GetFiles(savesDir, "*.json");

    var allSaveData = files.Select(file => {
      var content = _fs.File.ReadAllText(file);
      var data = JsonSerializer.Deserialize<T>(content, JSON_OPTIONS) ?? throw new SerializationException($"Failed to deserialize save data from {file}");

      return new GodotSave<T>(
        saveName: _fs.Path.GetFileNameWithoutExtension(file),
        data: data
      );
    });

    return allSaveData;
  }
}
