namespace Yolk.UI;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.FS;
using Yolk.Game;


[Meta(typeof(IAutoNode))]
public partial class LoadGamePanel : PanelContainer {
  public override void _Notification(int what) => this.Notify(what);

  [Export] private PackedScene GameSavePanelScene { get; set; } = default!;
  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private VBoxContainer GameSaves { get; set; } = default!;

  public void OnResolved() {
    UpdateGameSavesList();

    GameRepo.Saved += UpdateGameSavesList;
  }

  private void UpdateGameSavesList() {
    GameSaves.ClearChildren();
    var saves = GodotSave.GetAllSaves();

    foreach (var save in GodotSave.GetAllSaves()) {
      // var savePanel = GameSavePanelScene.Instantiate<GameSavePanel>();
      // savePanel.SaveInfo = save;
      // savePanel.AllowSave = false;
      // GameSaves.AddChild(savePanel);
    }
  }

}
