
namespace Yolk.FS;

using System.Data;
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

  public static string SaveDirectory => new FileSystem().Path.Join(OS.GetUserDataDir(), "saves");
  public static string SaveThumbnailDirectory => new FileSystem().Path.Join(OS.GetUserDataDir(), "saves", "thumbnails");

  public static void Initialize() {
    GodotSerialization.Setup();
    new FileSystem().Directory.CreateDirectory(SaveDirectory);
    new FileSystem().Directory.CreateDirectory(SaveThumbnailDirectory);
  }

  public static string GetSaveFilePath(string? saveName) => new FileSystem().Path.Join(OS.GetUserDataDir(), "saves", $"{saveName}.json");

  public static async Task Save<T>(T data, string? saveName) {
    GD.Print($"saving to file... (name: {saveName})");
    var fileSystem = new FileSystem();

    // Start screenshot capture asynchronously
    var screenshotTask = GodotScreenshot.SaveAsync($"{SaveThumbnailDirectory}/{saveName}.png", [1]);

    var path = GetSaveFilePath(saveName);
    try {
      // Serialize on a background thread
      var json = await Task.Run(() => JsonSerializer.Serialize(data, JSON_OPTIONS));

      // Wait for both operations to complete
      await Task.WhenAll(
        screenshotTask,
        fileSystem.File.WriteAllTextAsync(path, json)
      );
    }
    catch (Exception e) {
      GD.PushError($"Failed to save game: {e}");
    }
  }

  public static async Task<T> Load<T>(string? saveName) {
    GD.Print($"loading from file... (name: {saveName})");
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName);

    if (!fileSystem.File.Exists(path)) {
      GD.Print("No save file to load :'(");
      throw new FileNotFoundException("could not find save file");
    }

    var json = await fileSystem.File.ReadAllTextAsync(path);
    return JsonSerializer.Deserialize<T>(json, JSON_OPTIONS) ?? throw new SerializationException("could not serialize save game data");
  }

  public static void Delete(string saveName) {
    GD.Print($"deleting save file... (name: {saveName})");
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName);

    if (!fileSystem.File.Exists(path)) {
      GD.Print("No save file to delete :'(");
      return;
    }

    try {
      fileSystem.File.Delete(path);
      GD.Print($"Deleted save file: {path}");
    }
    catch (Exception e) {
      GD.PushError($"Failed to delete save file: {e}");
    }
  }

  public static bool Exists(string? saveName) {
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName);
    return fileSystem.File.Exists(path);
  }

  public static Texture2D GetThumbnail(string? saveName) {
    var fileSystem = new FileSystem();

    var imagePath = fileSystem.Path.Join(SaveThumbnailDirectory, $"{saveName}.png");

    var image = new Image();
    if (image.Load(imagePath) != Error.Ok) {
      GD.Print("Failed to load preview image.");
      return new PlaceholderTexture2D();
    }

    var texture = ImageTexture.CreateFromImage(image);
    return texture;
  }

  public static Error SaveThumbnail(string? saveName, Image image) {
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName);
    var imagePath = fileSystem.Path.Join(SaveThumbnailDirectory, $"{saveName}.png");

    return image.SavePng(imagePath);
  }

  public static IEnumerable<SaveFileInfo<T>> GetAllSaveInfo<T>() {
    var fileSystem = new FileSystem();
    var savesDir = fileSystem.Path.Join(OS.GetUserDataDir(), "saves");
    if (!fileSystem.Directory.Exists(savesDir)) {
      GD.Print("No save directory found.");
      return [];
    }

    var files = fileSystem.Directory.GetFiles(savesDir, "*.json");

    var allSaveData = files.Select(file => {
      var content = fileSystem.File.ReadAllText(file);
      var data = JsonSerializer.Deserialize<T>(content, JSON_OPTIONS) ?? throw new SerializationException($"Failed to deserialize save data from {file}");

      return new SaveFileInfo<T>(
        saveName: fileSystem.Path.GetFileNameWithoutExtension(file),
        data: data
      );
    });

    return allSaveData;
  }
}

public interface ISaveFileInfo {
  public string SaveName { get; }
  public string ThumbnailPath { get; }
  public bool HasThumbnail();
  public Texture2D GetThumbnail();
}

public class SaveFileInfo<T>(string saveName, T data) : ISaveFileInfo {
  public string SaveName { get; set; } = saveName;
  public string ThumbnailPath { get; set; } = $"{GodotSave.SaveThumbnailDirectory}/{saveName}.png";
  public T Data { get; set; } = data;

  public bool HasThumbnail() {
    var fileSystem = new FileSystem();
    return fileSystem.File.Exists(ThumbnailPath);
  }

  public Texture2D GetThumbnail() => GodotSave.GetThumbnail(SaveName);
}
