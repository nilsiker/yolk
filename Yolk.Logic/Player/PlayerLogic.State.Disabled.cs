
namespace Yolk.Logic.Player;

public partial class PlayerLogic
{

    public abstract partial record State
    {
        public partial record Disabled : State
        {

        }
    }
}
