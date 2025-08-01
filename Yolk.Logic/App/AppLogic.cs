namespace Yolk;

using Chickensoft.Introspection;
using Chickensoft.LogicBlocks;

public interface IAppLogic : ILogicBlock<AppLogic.State>;

[Meta]
[LogicBlock(typeof(State), Diagram = true)]
public partial class AppLogic
  : LogicBlock<AppLogic.State>,
    IAppLogic {
  public override Transition GetInitialState() => To<State>();

  public class Data {
    public required int BlackoutMinimumWaitTimeMs;
    public BlackoutCallback? Callback;
  }
}
