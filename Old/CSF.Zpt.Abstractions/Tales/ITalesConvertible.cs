using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Interface which may be used for types which implement custom code for conversions by the TALES system.
  /// </summary>
  public interface ITalesConvertible
  {
    /// <summary>
    /// Gets a boolean representation of the current instance.
    /// </summary>
    /// <returns><c>true</c>, or <c>false</c>, with logic dependant upon the implementation.</returns>
    bool AsBoolean();
  }
}

