namespace Yolk.UI;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.FS;
using Yolk.Game;

[Meta(typeof(IAutoNode))]
public partial class GameSaveSlot : PanelContainer {
  public override void _Notification(int what) => this.Notify(what);

  public bool AllowSave { get; set; } = true;
  public IGodotSaveInfo SaveInfo { get; set; } = default!;

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private TextureRect PreviewImage { get; set; } = default!;
  [Node] private Button SaveButton { get; set; } = default!;
  [Node] private Button LoadButton { get; set; } = default!;
  [Node] private Button DeleteButton { get; set; } = default!;
  [Node] private Label SaveNameLabel { get; set; } = default!;

  public void OnResolved() {
    SaveButton.Visible = AllowSave;

    PreviewImage.Texture = GodotSaver.GetThumbnail(SaveInfo.SaveName);

    SaveNameLabel.Text = SaveInfo.SaveName;

    LoadButton.Pressed += () => GameRepo.Load(SaveInfo.SaveName);
    DeleteButton.Pressed += () => GameRepo.Delete(SaveInfo.SaveName);
  }
}
