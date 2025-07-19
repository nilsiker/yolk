
namespace Yolk.Logic.Player;

public partial class PlayerLogic {

  public abstract partial record State {
    public partial record Enabled {
      public partial record Alive {
        public partial record OnWall : Alive {


        }
      }
    }
  }
}
