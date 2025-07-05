
namespace Yolk.Logic.Controls;

public partial class ControlsLogic {

  public abstract partial record State {
    public partial record Idle : State, IGet<Input.Listen> {
      public Idle() {
        OnAttach(() => Get<ControlsRepo>().Mapping += OnControlsMapping);
        OnDetach(() => Get<ControlsRepo>().Mapping -= OnControlsMapping);
      }

      private void OnControlsMapping(string action, EActionType actionType) => Input(new Input.Listen(action, actionType));

      public Transition On(in Input.Listen input) {
        Get<Data>().Action = input.Action;

        return input.ActionType switch {
          EActionType.Key => To<Listening.Key>(),
          _ => ToSelf(),
        };
      }
    }
  }
}

