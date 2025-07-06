namespace Yolk.Core;

/// <summary>
/// Interface for entities that can be killed.
/// <br/>
/// <br/>
/// Entity is responsible for calling any appropriate death logic.
/// </summary>
public interface IKillable {
  public void Kill();
}
