namespace Yolk.Generator {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable IDE0040 // Add accessibility modifiers
  /// <summary>
  /// Interface for classes that provide state information.
  /// Classes can implement this interface by decorating it with the StateInfo attribute.
  /// </summary>
  public interface IStateInfo {
    string Name { get; }
    string State { get; }
  }
}
#pragma warning restore IDE0040 // Add accessibility modifiers
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
