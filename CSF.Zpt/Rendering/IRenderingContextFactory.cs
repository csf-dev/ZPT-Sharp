using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Interface for a type which creates an instance of <see cref="RenderingContext"/> from the <see cref="RenderingOptions"/>.
  /// </summary>
  public interface IRenderingContextFactory
  {
    /// <summary>
    /// Create a context instance.
    /// </summary>
    RenderingContext Create();
  }
}

