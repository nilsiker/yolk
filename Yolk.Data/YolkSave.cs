namespace Yolk.Data;

using Chickensoft.SaveFileBuilder;
using Yolk.FS;

public interface IChunkRoot<T> where T : class {
  public ISaveChunk<T> Root { get; }
}

public class YolkSave<T>(string saveName, ESaveType saveType, ISaveChunk<T> root) : IChunkRoot<T> where T : class {
  public string SaveName { get; } = saveName;
  public ESaveType SaveType { get; } = saveType;
  public ISaveChunk<T> Root { get; } = root;
  public SaveFile<T> SaveFile { get; } = CreateSaveFile(root, saveName, ESaveType.Manual);
  public SaveFile<T> AutosaveFile { get; } = CreateSaveFile(root, saveName, ESaveType.Autosave);
  public SaveFile<T> QuicksaveFile { get; } = CreateSaveFile(root, saveName, ESaveType.Quicksave);

  private static SaveFile<T> CreateSaveFile(ISaveChunk<T> root, string saveName, ESaveType type) =>
    new(
      root: root,
      onSave: async data => await GodotSave.Save(data, saveName, type),
      onLoad: async () => await GodotSave.Load<T>(saveName, type)
    );

  public void Save() => SaveFile.Save();
  public void Load() => SaveFile.Load();
  public void Autosave() => AutosaveFile.Save();
  public void Autoload() => AutosaveFile.Load();
  public void Quicksave() => QuicksaveFile.Save();
  public void Quickload() => QuicksaveFile.Load();
}
