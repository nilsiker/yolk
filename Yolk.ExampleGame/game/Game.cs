namespace Yolk.Game;


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
  public SaveFile<GameData> SaveFile { get; set; } = default!;

  IGameRepo IProvide<IGameRepo>.Value() => GameRepo;
  ISaveChunk<GameData> IProvide<ISaveChunk<GameData>>.Value() => SaveFile.Root;

  public void Setup() {
    Logic = new();
    GameRepo = new GameRepo();
  }

  public void OnResolved() {
    GodotSave.Setup();

    SaveFile = new SaveFile<GameData>(
      root: new SaveChunk<GameData>(
        onSave: (chunk) => new() {
          WorldData = chunk.GetChunkSaveData<WorldData>(),
          PlayerData = chunk.GetChunkSaveData<PlayerData>()
        },
        onLoad: (chunk, data) => {
          chunk.LoadChunkSaveData(data.WorldData);
          chunk.LoadChunkSaveData(data.PlayerData);
        }),
      onSave: async data => {
        GetViewport().SetCanvasCullMaskBit(2, false);
        await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
        var image = GetViewport().GetTexture().GetImage();
        var error = GodotSave.SavePreviewImage(
          GameRepo.LastSaveName.Value,
          GameRepo.LastSaveType.Value,
          image);
        GetViewport().SetCanvasCullMaskBit(2, true);

        // Use LastSaveName and SaveType.Normal for now; you can adjust as needed
        await GodotSave.Save(data, GameRepo.LastSaveName.Value, GameRepo.LastSaveType.Value);
        Logic.Input(new GameLogic.Input.OnSaved());
      },
      onLoad: async () => {
        var data = await GodotSave.Load<GameData>(GameRepo.LastSaveName.Value, GameRepo.LastSaveType.Value);
        Logic.Input(new GameLogic.Input.OnLoaded());
        return data;
      });

    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
      .Handle((in GameLogic.Output.SetPauseMode output) => OnOutputSetPauseMode(output.Paused))
      .Handle((in GameLogic.Output.SaveGame output) => OnOutputSaveGame(output.SaveName))
      .Handle((in GameLogic.Output.LoadGame output) => OnOutputLoadGame(output.SaveName))
      .Handle((in GameLogic.Output.SetPauseMode output) => OnOutputSetPauseMode(output.Paused))
      .Handle((in GameLogic.Output.UpdateVisibility output) => OnOutputUpdateVisibility(output.Visible));

    Logic.Set(AppRepo);
    Logic.Set(GameRepo);
    Logic.Set(new GameLogic.Data { });

    Logic.Start();
    this.Provide();
  }

  private static void OnOutputSetCursorPosition(Vector2 cursorPosition) => Input.WarpMouse(cursorPosition);
  private void OnOutputSetPauseMode(bool paused) => GetTree().Paused = paused;
  private void OnOutputUpdateVisibility(bool visible) => Visible = visible;
  private void OnOutputSaveGame(string? saveName) => SaveFile.Save();
  private void OnOutputLoadGame(string? saveName) => SaveFile.Load();

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(HardCancel)) {
      Logic.Input(new GameLogic.Input.OnPauseUserInput());
    }
    else if (@event.IsActionPressed(Quicksave)) {
      Logic.Input(new GameLogic.Input.Save(GameRepo.LastSaveName.Value, ESaveType.Quicksave));
    }
    else if (@event.IsActionPressed(Quickload)) {
      if (GodotSave.Exists(GameRepo.LastSaveName.Value, ESaveType.Quicksave)) {
        Logic.Input(new GameLogic.Input.Load(GameRepo.LastSaveName.Value, ESaveType.Quicksave));
      }
      else {
        GD.PushWarning("Quickload failed: No quicksave found.");
      }
    }
  }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
