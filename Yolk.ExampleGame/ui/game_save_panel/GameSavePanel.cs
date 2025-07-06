namespace Yolk.UI;

using System.Linq;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.Data;
using Yolk.FS;
using Yolk.Game;

[Meta(typeof(IAutoNode))]
public partial class GameSavePanel : PanelContainer {
  public override void _Notification(int what) => this.Notify(what);

  public bool AllowSave { get; set; } = true;
  public string SaveFileName { get; set; } = default!;
  private ESaveType SaveType { get; set; } = default!;

  [Dependency] private IGameRepo GameRepo => this.DependOn<IGameRepo>();

  [Node] private TextureRect PreviewImage { get; set; } = default!;
  [Node] private Button SaveButton { get; set; } = default!;
  [Node] private Button LoadButton { get; set; } = default!;
  [Node] private Button DeleteButton { get; set; } = default!;
  [Node] private Label SaveNameLabel { get; set; } = default!;
  [Node] private Label SaveTypeLabel { get; set; } = default!;

  public void OnResolved() {
    SaveButton.Visible = AllowSave;

    SaveType = SaveFileName switch {
      _ when SaveFileName.EndsWith("quicksave") => ESaveType.Quicksave,
      _ when SaveFileName.EndsWith("autosave") => ESaveType.Autosave,
      _ => ESaveType.Manual
    };

    var saveName = string.Join("", SaveFileName.Split('_').SkipLast(1));

    PreviewImage.Texture = GodotSave.GetPreviewImage(saveName, SaveType);

    SaveNameLabel.Text = saveName;
    SaveNameLabel.Visible = saveName.Length > 0;
    SaveTypeLabel.Text = SaveType.ToString();


    LoadButton.Pressed += () => GameRepo.Load(saveName, SaveType);
  }
}
