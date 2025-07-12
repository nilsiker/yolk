namespace Yolk.FS;

using System.IO.Abstractions;
using Godot;

public interface IGodotSaveInfo {
  public string SaveName { get; }
  public string ThumbnailPath { get; }
  public bool HasThumbnail();
  public Texture2D GetThumbnail();
}

public class GodotSave<T>(string saveName, T data) : IGodotSaveInfo {
  private static readonly FileSystem _fs = new();

  public string SaveName { get; set; } = saveName;
  public string ThumbnailPath { get; set; } = $"{GodotSaver.SaveThumbnailDirectory}/{saveName}.png";
  public T Data { get; set; } = data;

  public bool HasThumbnail() => _fs.File.Exists(ThumbnailPath);
  public Texture2D GetThumbnail() => GodotSaver.GetThumbnail(SaveName);
}
