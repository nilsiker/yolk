namespace Yolk.ExampleGame.Level;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Godot;
using Yolk.Data;
using Yolk.Game;
using Yolk.Level;
using Yolk.World;


public interface IWorld : INode, IProvide<IWorldRepo>, IStateInfo;

[Meta(typeof(IAutoNode))]
public partial class World : Node, IWorld {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private ISaveChunk<GameData> GameSaveChunk => this.DependOn<ISaveChunk<GameData>>();
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  IWorldRepo IProvide<IWorldRepo>.Value() => WorldRepo;

  [Export] private Level _level = default!; // TODO don't export this, handle programmatically?
  [Node] private Player Player { get; set; } = default!;
  private IWorldRepo WorldRepo { get; set; } = new WorldRepo();
  private WorldLogic Logic { get; set; } = new WorldLogic();
  private WorldLogic.IBinding Binding { get; set; } = default!;
  private ISaveChunk<WorldData> LevelSaveChunk { get; set; } = default!;
  string IStateInfo.Name => Name;
  public string State => Logic.Value.ToString();

  public void OnResolved() {
    LevelSaveChunk = new SaveChunk<WorldData>(
      onSave: chunk => new WorldData {
        CurrentLevelName = _level.Name
      },
      onLoad: (chunk, data) => Logic.Input(new WorldLogic.Input.RequestLevelTransition("", data.CurrentLevelName, true)));

    GameSaveChunk.AddChunk(LevelSaveChunk);

    Binding = Logic.Bind();

    Binding
      .Handle((in WorldLogic.Output.LoadLevel output) => OnOutputLoadLevel(output.LevelName));

    Logic.Set(AppRepo);
    Logic.Set(GameRepo);
    Logic.Set(WorldRepo);
    Logic.Set(new WorldLogic.Data {
      LevelToLoad = Level.DEBUG_LEVEL,
      PreviousLevelName = Level.DEBUG_LEVEL,
      SkipBlackout = false
    });

    Logic.Start();
    this.Provide();
  }
  private void OnOutputLoadLevel(string levelName) {
    var loadingFromLevel = _level?.Name ?? Level.DEBUG_LEVEL;
    var scene = GD.Load<PackedScene>(Level.GetLevelPath(levelName));

    if (_level is not null) {
      _level.Name += "_Removing";
      _level.QueueFree();
    }

    _level = scene.Instantiate<Level>();

    AddChild(_level, true);

    var landing = _level.GetLanding(loadingFromLevel);
    if (landing is null) {
      GD.PushWarning("Landing was null for ", loadingFromLevel);
    }

    Logic.Input(new WorldLogic.Input.OnLevelLoaded(landing?.GlobalTransform));
  }

  public override void _Ready() => AddToGroup("state");

  public override void _ExitTree() => Binding.Dispose();

}

