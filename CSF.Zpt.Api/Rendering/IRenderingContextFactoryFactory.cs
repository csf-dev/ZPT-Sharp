using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which creates instances of <see cref="IRenderingContextFactory"/> via reflection.
  /// </summary>
  public interface IRenderingContextFactoryFactory
  {
    /// <summary>
    /// Create the factory from a fully-qualified type name.
    /// </summary>
    /// <param name="className">The class name for the desired factory instance.</param>
    IRenderingContextFactory Create(string className);
  }
}

