namespace Yolk.Game;

using System;

using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Godot;
using Yolk.Data;
using Yolk.FS;
using Yolk.Generator;

public interface IGame : IControl,
  IProvide<IGameRepo>,
  IProvide<ISaveChunk<GameData>>;

[InputMap]
[Meta(typeof(IAutoNode))]
[StateInfo]
public partial class Game : Control, IGame {
  public override void _Notification(int what) => this.Notify(what);
  [Dependency] private IAppRepo AppRepo => this.DependOn<IAppRepo>();

  public IGameRepo GameRepo { get; set; } = default!;
  // NOTE game currently acts as a service layer for more generally available repos
  private GameLogic Logic { get; set; } = new();
  private GameLogic.IBinding Binding { get; set; } = default!;
  public YolkSave<GameData> SaveFile { get; set; } = default!;

  IGameRepo IProvide<IGameRepo>.Value() => GameRepo;
  ISaveChunk<GameData> IProvide<ISaveChunk<GameData>>.Value() => SaveFile.Root;

  public void Setup() {
    Logic = new();
    GameRepo = new GameRepo();
  }

  public void OnResolved() {
    GodotSaver.Initialize();

    SaveFile = new YolkSave<GameData>(
      "Default",
      new SaveChunk<GameData>(
        onSave: (chunk) => new GameData {
          WorldData = chunk.GetChunkSaveData<WorldData>(),
          PlayerData = chunk.GetChunkSaveData<PlayerData>()
        },
        onLoad: (chunk, data) => {
          chunk.LoadChunkSaveData(data.WorldData);
          chunk.LoadChunkSaveData(data.PlayerData);
        }
      )
    );

    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
      .Handle((in GameLogic.Output.SetPauseMode output) => OnOutputSetPauseMode(output.Paused))
      .Handle((in GameLogic.Output.SaveGame output) => OnOutputSaveGame(output.SaveName))
      .Handle((in GameLogic.Output.LoadGame output) => OnOutputLoadGame(output.SaveName))
      .Handle((in GameLogic.Output.DeleteSave output) => OnOutputDeleteSave(output.SaveName))
      .Handle((in GameLogic.Output.Autosave output) => OnOutputAutosave())
      .Handle((in GameLogic.Output.Autoload output) => OnOutputAutoload())
      .Handle((in GameLogic.Output.Quicksave output) => OnOutputQuicksave())
      .Handle((in GameLogic.Output.Quickload output) => OnOutputQuickload())
      .Handle((in GameLogic.Output.SetPauseMode output) => OnOutputSetPauseMode(output.Paused))
      .Handle((in GameLogic.Output.UpdateVisibility output) => OnOutputUpdateVisibility(output.Visible));

    Logic.Set(AppRepo);
    Logic.Set(GameRepo);
    Logic.Set(new GameLogic.Data {
      SaveName = SaveFile.SaveName,
      LoadType = ELoadType.Manual,
    });

    Logic.Start();
    this.Provide();
  }

  private static void OnOutputDeleteSave(string saveName) => GodotSaver.Delete(saveName);
  private static void OnOutputSetCursorPosition(Vector2 cursorPosition) => Input.WarpMouse(cursorPosition);
  private void OnOutputSetPauseMode(bool paused) => GetTree().Paused = paused;
  private void OnOutputUpdateVisibility(bool visible) => Visible = visible;
  private void OnOutputSaveGame(string saveName) {
    SaveFile.SaveName = saveName;
    SaveFile.Save().ContinueWith((_) => Logic.Input(new GameLogic.Input.OnSaved()));
  }

  private void OnOutputLoadGame(string saveName) {
    SaveFile.SaveName = saveName;
    SaveFile.Load();
  }

  private void OnOutputAutosave() => SaveFile.Autosave().ContinueWith((_) => Logic.Input(new GameLogic.Input.OnSaved()));
  private void OnOutputAutoload() => SaveFile.Autoload();
  private void OnOutputQuicksave() => SaveFile.Quicksave().ContinueWith((_) => Logic.Input(new GameLogic.Input.OnSaved()));
  private void OnOutputQuickload() => SaveFile.Quickload();

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(HardCancel)) {
      Logic.Input(new GameLogic.Input.OnPauseUserInput());
    }
    else if (@event.IsActionPressed(Quicksave)) {
      Logic.Input(new GameLogic.Input.Quicksave());
    }
    else if (@event.IsActionPressed(Quickload)) {
      if (SaveFile.HasQuicksave) {
        Logic.Input(new GameLogic.Input.Quickload());
      }
      else {
        GD.PrintErr("Cannot quicksave, save file does not exist.");
      }
    }
  }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
