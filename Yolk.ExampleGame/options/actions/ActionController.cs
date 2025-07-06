namespace Yolk.Options.Actions;

using System;
using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using Godot;
using Yolk.FS;


public interface IActionRepo {
  public event Action<string, InputEvent>? ActionMapped;
  public event Action? DefaultsRestored;
  public void MapAction(string actionName, InputEvent inputEvent);
  public void BroadcastDefaultsRestored();
}

public class ActionRepo : IActionRepo {
  // Implement the methods defined in IActionsRepo
  public event Action<string, InputEvent>? ActionMapped;
  public event Action? DefaultsRestored;
  public void MapAction(string actionName, InputEvent inputEvent) => ActionMapped?.Invoke(actionName, inputEvent);
  public void BroadcastDefaultsRestored() => DefaultsRestored?.Invoke();
}

[Meta(typeof(IAutoNode))]
public partial class ActionController : Node, IProvide<IActionRepo> {
  public override void _Notification(int what) => this.Notify(what);

  private ActionRepo ActionRepo { get; set; } = new();

  public IActionRepo Value() => ActionRepo;


  public void OnResolved() {
    ActionRepo.ActionMapped += OnActionMapped;

    GodotConfig.ImportInputMap();

    this.Provide();
  }


  private void OnActionMapped(string actionName, InputEvent inputEvent) {
    InputMap.ActionEraseEvents(actionName);
    InputMap.ActionAddEvent(actionName, inputEvent);

    GodotConfig.WriteMappedAction(actionName);
  }

  public override void _ExitTree() {
    ActionRepo.ActionMapped -= OnActionMapped;
    base._ExitTree();
  }
}
