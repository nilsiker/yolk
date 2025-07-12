namespace Yolk.UI;

using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Data;
using Yolk.FS;
using Yolk.Game;


[Meta(typeof(IAutoNode))]
public partial class SaveGamePanel : PanelContainer {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private PackedScene GameSaveSlotScene { get; set; } = default!;
  [Export] private bool _allowSave = false;
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private VBoxContainer Saves { get; set; } = default!;
  [Node] private Control NewSaveBox { get; set; } = default!;
  [Node] private LineEdit SaveNameInput { get; set; } = default!;
  [Node] private Button NewSaveButton { get; set; } = default!;

  public void OnResolved() {
    InitGameSavesList();

    GameRepo.Saved += UpdateGameSavesListDeferred;
    GameRepo.SaveDeleted += OnGameSaveDeleted;

    NewSaveBox.Visible = _allowSave;

    NewSaveButton.Pressed += OnNewSaveButtonPressed;
    NewSaveButton.Disabled = true;
    SaveNameInput.TextChanged += OnSaveNameInputTextChanged;
    SaveNameInput.TextSubmitted += OnSaveNameInputTextSubmitted;
  }

  private void OnSaveNameInputTextSubmitted(string _) {
    if (!NewSaveButton.Disabled) {
      OnNewSaveButtonPressed();
    }
  }


  private void OnGameSaveDeleted(string saveName) {
    var slot = Saves.GetChildren()
          .OfType<GameSaveSlot>()
          .FirstOrDefault(c => c.SaveInfo?.SaveName == saveName);

    if (slot is not null) {
      slot.Name += "_REMOVING";
      slot.QueueFree();
    }
    else {
      GD.PrintErr($"Save slot for '{saveName}' not found.");
    }
  }
  private void OnSaveNameInputTextChanged(string text) {
    var saveExists = GodotSaver.Exists(text.Trim());
    var tooLong = text.Length > 24;
    var empty = string.IsNullOrWhiteSpace(text);
    NewSaveButton.Disabled = empty || tooLong || saveExists;
  }

  private void OnNewSaveButtonPressed() {
    GameRepo.Save(SaveNameInput.Text.Trim());
    SaveNameInput.Text = string.Empty;
    NewSaveButton.Disabled = true;
  }

  private void UpdateGameSavesListDeferred(string saveName)
    => Callable.From(() => UpdateGameSavesList(saveName)).CallDeferred();

  private void UpdateGameSavesList(string saveName) {
    var slot = Saves.GetChildren()
          .OfType<GameSaveSlot>()
          .FirstOrDefault(c => c.SaveInfo?.SaveName == saveName);

    if (slot is null) {
      var saveInfo = GodotSaver.GetSaveInfo<GameData>(saveName);
      if (saveInfo is null) {
        GD.PrintErr($"Save info for '{saveName}' not found.");
        return;
      }

      if (saveInfo.SaveName == "Autosave") {
        AddAutosaveGameSlot(saveInfo);
      }
      else {
        AddGameSlot(saveInfo);
      }
    }
    else {
      slot.Update(saveName);
    }
  }

  private void InitGameSavesList() {
    Saves.ClearChildren();

    foreach (var save in GodotSaver.GetAllSaveInfo<GameData>()) {
      if (save.SaveName == "Autosave") {
        AddAutosaveGameSlot(save);
      }
      else {
        AddGameSlot(save);
      }
    }
  }

  private void AddAutosaveGameSlot(GodotSave<GameData> save) {
    var savePanel = GameSaveSlotScene.Instantiate<GameSaveSlot>();
    savePanel.AllowSave = false;
    savePanel.SaveInfo = save;
    Saves.AddChild(savePanel);
    Saves.MoveChild(savePanel, 0);
  }

  private void AddGameSlot(GodotSave<GameData> save) {
    var savePanel = GameSaveSlotScene.Instantiate<GameSaveSlot>();
    savePanel.AllowSave = _allowSave;
    savePanel.SaveInfo = save;
    Saves.AddChild(savePanel);
  }
}
