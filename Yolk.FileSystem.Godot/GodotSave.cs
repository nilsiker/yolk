
namespace Yolk.FS;

using System.IO.Abstractions;
using System.Runtime.Serialization;
using System.Text.Json;
using Chickensoft.Collections;
using Chickensoft.Serialization;
using Chickensoft.Serialization.Godot;
using Godot;
using Yolk.Data;


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

  private static string SaveDirectory => new FileSystem().Path.Join(OS.GetUserDataDir(), "saves");

  public static void Setup() {
    GodotSerialization.Setup();
    new FileSystem().Directory.CreateDirectory(SaveDirectory);
  }

  private static string GetSaveFileName(string? requestedName, ESaveType type = ESaveType.Manual)
    => type == ESaveType.Manual
      ? string.IsNullOrWhiteSpace(requestedName) ? "Save" : requestedName
      : $"{(string.IsNullOrWhiteSpace(requestedName) ? "" : requestedName + "_")}{type.ToString().ToLowerInvariant()}";

  public static string GetSaveFilePath(string? saveName, ESaveType type = ESaveType.Manual) {
    var fileName = GetSaveFileName(saveName, type);
    return new FileSystem().Path.Join(OS.GetUserDataDir(), "saves", $"{fileName}.json");
  }

  public static async Task Save<T>(T data, string? saveName, ESaveType type = ESaveType.Manual) {
    GD.Print($"saving to file... (name: {saveName}, type: {type})");
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName, type);
    try {
      var json = JsonSerializer.Serialize(data, JSON_OPTIONS);
      await fileSystem.File.WriteAllTextAsync(path, json);
    }
    catch (Exception e) {
      GD.PushError($"Failed to save game: {e}");
    }
  }

  public static async Task<T> Load<T>(string? saveName, ESaveType type = ESaveType.Manual) {
    GD.Print($"loading from file... (name: {saveName}, type: {type})");
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName, type);
    if (!fileSystem.File.Exists(path)) {
      GD.Print("No save file to load :'(");
      throw new SerializationException("could not find save file");
    }
    var json = await fileSystem.File.ReadAllTextAsync(path);
    return JsonSerializer.Deserialize<T>(json, JSON_OPTIONS) ?? throw new SerializationException("could not serialize save game data");
  }

  public static bool Exists(string? saveName, ESaveType type = ESaveType.Manual) {
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName, type);
    return fileSystem.File.Exists(path);
  }

  public static Texture2D GetPreviewImage(string? saveName, ESaveType type = ESaveType.Manual) {
    var fileSystem = new FileSystem();

    var imagePath = fileSystem.Path.Join(SaveDirectory, $"{GetSaveFileName(saveName, type)}.png");
    if (!fileSystem.File.Exists(imagePath)) {
      GD.Print("No preview image found for save file.");
      return new PlaceholderTexture2D();
    }

    var image = new Image();
    if (image.Load(imagePath) != Error.Ok) {
      GD.Print("Failed to load preview image.");
      return new PlaceholderTexture2D();
    }

    var texture = ImageTexture.CreateFromImage(image);
    return texture;
  }

  public static Error SavePreviewImage(string? saveName, ESaveType type, Image image) {
    var fileSystem = new FileSystem();
    var path = GetSaveFilePath(saveName, type);
    var imagePath = fileSystem.Path.Join(SaveDirectory, $"{GetSaveFileName(saveName, type)}.png");

    return image.SavePng(imagePath);
  }

  public static string[] GetAllSaves() {
    var fileSystem = new FileSystem();
    var savesDir = fileSystem.Path.Join(OS.GetUserDataDir(), "saves");
    if (!fileSystem.Directory.Exists(savesDir)) {
      GD.Print("No save directory found.");
      return [];
    }

    var files = fileSystem.Directory.GetFiles(savesDir, "*.json");
    return [.. files.Select(file => fileSystem.Path.GetFileNameWithoutExtension(file))];
  }
}
