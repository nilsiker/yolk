namespace Yolk.UI;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Data;
using Yolk.FS;
using Yolk.Game;

[Meta(typeof(IAutoNode))]
public partial class GameSaveSlot : PanelContainer {
  public override void _Notification(int what) => this.Notify(what);

  private IGodotSaveInfo? _saveInfo;

  public bool AllowSave { get; set; } = true;

  public IGodotSaveInfo? SaveInfo {
    get => _saveInfo;
    set {
      _saveInfo = value;
      if (IsInsideTree()) {
        UpdateVisuals();
      }
    }
  }

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private TextureRect PreviewImage { get; set; } = default!;
  [Node] private Button SaveButton { get; set; } = default!;
  [Node] private Button LoadButton { get; set; } = default!;
  [Node] private Button DeleteButton { get; set; } = default!;
  [Node] private Label SaveNameLabel { get; set; } = default!;

  public void OnResolved() {
    SaveButton.Visible = AllowSave;

    if (SaveInfo is null) {
      GD.PushWarning("SaveInfo is null, cannot initialize GameSaveSlot.");
      return;
    }

    UpdateVisuals();

    SaveButton.Pressed += () => GameRepo.Save(SaveInfo.SaveName);
    LoadButton.Pressed += () => GameRepo.Load(SaveInfo.SaveName);
    DeleteButton.Pressed += () => GameRepo.Delete(SaveInfo.SaveName);
  }

  public void Update(string saveName) {
    SaveInfo = GodotSaver.GetSaveInfo<GameData>(saveName);

    UpdateVisuals();
  }

  public void UpdateVisuals() {
    if (SaveInfo is null) {
      GD.PrintErr($"No save info for node {Name}.");
      return;
    }

    PreviewImage.Texture = GodotSaver.GetThumbnail(SaveInfo.SaveName);
    SaveNameLabel.Text = SaveInfo.SaveName;
  }
}
