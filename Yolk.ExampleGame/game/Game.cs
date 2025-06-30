namespace Yolk.Game;

using System;
using Chickensoft.AutoInject;
using Chickensoft.GodotNodeInterfaces;
using Chickensoft.Introspection;
using Chickensoft.SaveFileBuilder;
using Chickensoft.Serialization.Godot;
using Godot;
using Yolk.Data;
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

  private IGameRepo GameRepo { get; set; } = default!;
  // NOTE game currently acts as a service layer for more generally available repos
  private GameLogic Logic { get; set; } = default!;
  private GameLogic.IBinding Binding { get; set; } = default!;
  private int Slot { get; set; } = -1;
  public SaveFile<GameData> SaveFile { get; set; } = default!;

  IGameRepo IProvide<IGameRepo>.Value() => GameRepo;
  ISaveChunk<GameData> IProvide<ISaveChunk<GameData>>.Value() => SaveFile.Root;

  public void Setup() {
    Logic = new();
    GameRepo = new GameRepo();
  }

  public void OnResolved() {
    GodotSerialization.Setup();
    SaveFile = new SaveFile<GameData>(
      root: new SaveChunk<GameData>(
        onSave: (chunk) => new() {

          WorldData = chunk.GetChunkSaveData<WorldData>()
        },
        onLoad: (chunk, data) => {
          chunk.LoadChunkSaveData(data.WorldData);
        }),
      onSave: async data => {
        GetViewport().SetCanvasCullMaskBit(2, false);
        await ToSignal(RenderingServer.Singleton, RenderingServer.SignalName.FramePostDraw);
        var image = GetViewport().GetTexture().GetImage();
        var error = image.SavePng($"{OS.GetUserDataDir()}/slot{Slot}.png");
        GetViewport().SetCanvasCullMaskBit(2, true);

        await Save.SaveGame(data, Slot);
      },
      onLoad: async () => {
        var data = await Save.LoadGame(Slot);
        Logic.Input(new GameLogic.Input.OnLoaded()); // TODO this does not get called if load fails. Needs more robust load logic...
        return data;
      });

    Binding = Logic.Bind();

    // Bind functions to state outputs here
    Binding
      .Handle((in GameLogic.Output.SetPauseMode output) => OnOutputSetPauseMode(output.Paused))
      .Handle((in GameLogic.Output.SetSlot output) => OnOutputSetSlot(output.Slot))
      .Handle((in GameLogic.Output.SaveGame output) => OnOutputSaveGame(output.Slot))
      .Handle((in GameLogic.Output.LoadGame output) => OnOutputLoadGame(output.Slot))
      .Handle((in GameLogic.Output.SetPauseMode output) => OnOutputSetPauseMode(output.Paused))
      .Handle((in GameLogic.Output.UpdateVisibility output) => OnOutputUpdateVisibility(output.Visible));

    Logic.Set(AppRepo);
    Logic.Set(GameRepo);
    Logic.Set(new GameLogic.Data {
      Slot = Slot
    });

    Logic.Start();
    this.Provide();
  }

  private void OnOutputSetSlot(int slot) => Slot = slot;
  private static void OnOutputSetCursorPosition(Vector2 cursorPosition) => Input.WarpMouse(cursorPosition);
  private void OnOutputSetPauseMode(bool paused) => GetTree().Paused = paused;
  private void OnOutputUpdateVisibility(bool visible) => Visible = visible;
  private void OnOutputSaveGame(int slot) => SaveFile.Save();
  private void OnOutputLoadGame(int slot) => SaveFile.Load();

  public override void _Ready() => AddToGroup("state");

  public override void _UnhandledInput(InputEvent @event) {
    if (@event.IsActionPressed(Pause)) {
      Logic.Input(new GameLogic.Input.OnPauseUserInput());
    }
    else if (@event.IsActionPressed(Quicksave)) {
      Logic.Input(new GameLogic.Input.Save());
    }
    else if (@event.IsActionPressed(Quickload)) {
      Logic.Input(new GameLogic.Input.Load());
    }
  }

  public void OnExitTree() {
    Logic.Stop();
    Binding.Dispose();
  }
}
