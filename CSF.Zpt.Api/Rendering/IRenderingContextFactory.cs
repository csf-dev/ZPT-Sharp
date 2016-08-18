using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which creates an instance of <see cref="RenderingContext"/> from the
  /// <see cref="IRenderingOptions"/>.
  /// </summary>
  public interface IRenderingContextFactory
  {
    /// <summary>
    /// Create a context instance.
    /// </summary>
    RenderingContext Create(ZptElement element, IRenderingOptions options);

    /// <summary>
    /// Adds a keyword option to contexts created by the current instance.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    void AddKeywordOption(string key, string value);
  }
}

