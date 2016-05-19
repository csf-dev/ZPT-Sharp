using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which creates instances of <see cref="IRenderingContextFactory"/> via reflection.
  /// </summary>
  public class RenderingContextFactoryFactory : IRenderingContextFactoryFactory
  {
    /// <summary>
    /// Create the factory from a fully-qualified type name.
    /// </summary>
    /// <param name="className">The class name for the desired factory instance.</param>
    public IRenderingContextFactory Create(string className)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }
  }
}

