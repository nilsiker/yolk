namespace Yolk.UI;

using Chickensoft.LogicBlocks;

public partial class OptionsMenuLogic {
  public abstract partial record State : StateLogic<State> {
    public State() {
      OnAttach(() => Get<IOptionsRepo>().UIVisible.Sync += OnOptionsUIVisibleSync);
      OnDetach(() => Get<IOptionsRepo>().UIVisible.Sync -= OnOptionsUIVisibleSync);
    }

    private void OnOptionsUIVisibleSync(bool visible) {
      if (visible) {
        Input(new Input.Show());
      }
      else {
        Input(new Input.Hide());
      }
    }
  }
}
