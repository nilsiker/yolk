
namespace Yolk.Generator {
  using System;

  /// <summary>
  /// Attribute to mark classes that should be processed by the StateInfoGenerator.
  /// </summary>
  [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
  public sealed class StateInfoAttribute : Attribute { }
}
