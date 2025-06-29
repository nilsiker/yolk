namespace Yolk.World;

using Godot;

public interface IWorldRepo {
  public event Action<string>? LevelLoadRequested;
  public event Action<Transform3D?>? LevelLoaded;

  public void RequestLevelLoad(string toLevelName);
  public void BroadcastLevelLoaded(Transform3D? landingTransform);
}

public class WorldRepo : IWorldRepo {
  public event Action<string>? LevelLoadRequested;
  public event Action<Transform3D?>? LevelLoaded;

  public void BroadcastLevelLoaded(Transform3D? landingTransform) => LevelLoaded?.Invoke(landingTransform);
  public void RequestLevelLoad(string toLevelName) => LevelLoadRequested?.Invoke(toLevelName);
}
