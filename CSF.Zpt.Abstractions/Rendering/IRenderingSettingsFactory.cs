using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Factory type for creating instances of <see cref="IRenderingSettings"/>.
  /// </summary>
  public interface IRenderingSettingsFactory
  {
    /// <summary>
    /// Creates the settings from the given rendering options.
    /// </summary>
    /// <returns>The settings.</returns>
    /// <param name="options">Options.</param>
    IRenderingSettings CreateSettings(IRenderingOptions options);
  }
}

