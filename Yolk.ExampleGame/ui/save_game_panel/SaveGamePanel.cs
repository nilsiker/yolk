namespace Yolk.UI;

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

  [Node] private VBoxContainer GameSaves { get; set; } = default!;
  [Node] private Control NewSaveBox { get; set; } = default!;

  public void OnResolved() {
    UpdateGameSavesList();

    GameRepo.GameSavesUpdated += UpdateGameSavesList;

    NewSaveBox.Visible = _allowSave;
  }

  private void UpdateGameSavesList() {
    Callable.From(GameSaves.ClearChildren).CallDeferred();

    foreach (var save in GodotSave.GetAllSaveInfo<GameData>()) {
      var savePanel = GameSaveSlotScene.Instantiate<GameSaveSlot>();
      savePanel.AllowSave = _allowSave; // Disable saving in load panel
      savePanel.SaveInfo = save;

      Callable.From(() => GameSaves.AddChild(savePanel)).CallDeferred();
    }
  }
}
