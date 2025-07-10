namespace Yolk.Data;

using Chickensoft.SaveFileBuilder;
using Godot;
using Yolk.FS;

public interface ISaveInfo {
  public string SaveName { get; }
  public bool HasQuicksave { get; }
}

public interface IChunkRoot<T> where T : class {
  public ISaveChunk<T> Root { get; }
}

public class YolkSave<T> : ISaveInfo, IChunkRoot<T> where T : class {
  public string SaveName { get; set; }
  public ISaveChunk<T> Root { get; }
  public SaveFile<T> SaveFile { get; }
  public SaveFile<T> AutosaveFile { get; }
  public SaveFile<T> QuicksaveFile { get; }

  public bool HasQuicksave => _quickSaveData is not null;

  private T? _quickSaveData;

  public YolkSave(string saveName, ISaveChunk<T> root) {
    SaveName = saveName;
    Root = root;
    SaveFile = CreateSaveFile(root, saveName);
    AutosaveFile = CreateAutosaveFile(root, saveName);
    QuicksaveFile = CreateQuicksaveFile(root);
  }

  private static SaveFile<T> CreateSaveFile(ISaveChunk<T> root, string saveName) =>
    new(
      root: root,
      onSave: async data => await GodotSave.Save(data, saveName),
      onLoad: async () => await GodotSave.Load<T>(saveName)
    );

  private static SaveFile<T> CreateAutosaveFile(ISaveChunk<T> root, string saveName) =>
    new(
      root: root,
      onSave: async data => await GodotSave.Save(data, $"{saveName}_auto"),
      onLoad: async () => await GodotSave.Load<T>($"{saveName}_auto")
    );

  private SaveFile<T> CreateQuicksaveFile(ISaveChunk<T> root) =>
    new(
      root: root,
      onSave: async data => { _quickSaveData = data; await Task.CompletedTask; GD.Print("Quicksave data saved."); },
      onLoad: async () => await Task.FromResult(_quickSaveData)
    );

  public void Save() => SaveFile.Save();
  public void Load() => SaveFile.Load();
  public void Autosave() => AutosaveFile.Save();
  public void Autoload() => AutosaveFile.Load();
  public void Quicksave() => QuicksaveFile.Save();
  public void Quickload() {
    if (_quickSaveData is not null) {
      QuicksaveFile.Load();
    }
    else {
      GD.PushError("No quicksave data available.");
    }
  }
}
