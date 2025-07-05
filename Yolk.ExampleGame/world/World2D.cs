namespace Yolk.ExampleGame.Level;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Godot;
using Yolk.Data;
using Yolk.Game;
using Yolk.Generator;
using Yolk.Level;
using Yolk.Logic.World;
using Yolk.World;


public interface IWorld2D : INode, IProvide<IWorldRepo>;

[StateInfo]
[Meta(typeof(IAutoNode))]
public partial class World2D : Node, IWorld2D {
  public override void _Notification(int what) => this.Notify(what);

  [Dependency] private ISaveChunk<GameData> GameSaveChunk => this.DependOn<ISaveChunk<GameData>>();
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  IWorldRepo IProvide<IWorldRepo>.Value() => WorldRepo;

  [Export] private PackedScene _startScene = default!;

  [Node] private Player Player { get; set; } = default!;
  [Node] private Node2D Levels { get; set; } = default!;

  private IWorldRepo WorldRepo { get; set; } = new WorldRepo();
  private WorldLogic Logic { get; set; } = new WorldLogic();
  private WorldLogic.IBinding Binding { get; set; } = default!;
  private ISaveChunk<WorldData> LevelSaveChunk { get; set; } = default!;

  public void OnResolved() {
    LevelSaveChunk = new SaveChunk<WorldData>(
      onSave: chunk => new WorldData {
        CurrentLevelName = "Level_0" // TODO sort this out!
      },
      onLoad: (chunk, data) => Logic.Input(new WorldLogic.Input.Transition(data.CurrentLevelName)));

    GameSaveChunk.AddChunk(LevelSaveChunk);
    Binding = Logic.Bind();

    Binding
      .Handle((in WorldLogic.Output.TransitionLevel output) => OnOutputTransitionLevel(output.LevelName, output.FromLevelName))
      .Handle((in WorldLogic.Output.Clear _) => OnOutputClear());

    Logic.Set(AppRepo);
    Logic.Set(GameRepo);
    Logic.Set(WorldRepo);
    Logic.Set(new WorldLogic.Data {
      LevelToLoad = Level2D.DEBUG_LEVEL,
      PreviousLevelName = Level2D.DEBUG_LEVEL,
    });

    Logic.Start();
    this.Provide();
  }

  private void OnOutputClear() {
    foreach (var levelChild in Levels.GetChildren()) {
      levelChild.Name += "_Removing";
      levelChild.QueueFree();
    }
  }

  private void OnOutputTransitionLevel(string levelName, string? fromLevelName) {
    var pathLevelTo = Level2D.GetLevelPath(levelName);
    var scene = GD.Load<PackedScene>(pathLevelTo);

    var level = scene.Instantiate<Level2D>();

    Levels.AddChild(level, true);

    var entrypoint = level.GetEntrypointTransform(fromLevelName ?? "Default");

    if (entrypoint is not null) {
      var entrypointTransform = new Transform(entrypoint.GlobalPosition.Vec2(), entrypoint.RotationDegrees);
      Logic.Input(new WorldLogic.Input.OnTransitioned(entrypointTransform));
    }
    else {
      GD.PushError("TRANSITION FAILED: entrypoint not found for ", fromLevelName ?? "Default");
    }
  }

  public override void _ExitTree() => Binding.Dispose();

}

