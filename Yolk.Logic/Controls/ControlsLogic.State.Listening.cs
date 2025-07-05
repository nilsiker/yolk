
namespace Yolk.Logic.Controls;

using Chickensoft.LogicBlocks;
using Godot;

public partial class ControlsLogic {

  public abstract partial record State {
    public partial record Listening : State, IGet<Input.Cancel> {
      public Listening() {
        this.OnExit(() => Get<Data>().Action = null);
      }


      public Transition On(in Input.Cancel input) => To<Idle>();

      public partial record Key : Listening, IGet<Input.PressKey> {
        public Transition On(in Input.PressKey input) {
          var action = Get<Data>().Action;

          if (action is not null) {
            Output(new Output.ActionMapped(action, input.Key));
          }
          else {
            GD.PushWarning("Action should not be null when in listening state");
          }

          return To<Idle>();
        }
      }
    }
  }
}

