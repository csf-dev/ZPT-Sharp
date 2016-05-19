using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which creates instances of <see cref="IContextVisitor"/> via reflection.
  /// </summary>
  public class ContextVisitorFactory : IContextVisitorFactory
  {
    /// <summary>
    /// Create the visitor from a fully-qualified type name.
    /// </summary>
    /// <param name="className">The class name for the desired visitor instance.</param>
    public IContextVisitor Create(string className)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    /// <summary>
    /// Create a collection of visitors from a semicolon-separated list of fully-qualified type name.
    /// </summary>
    /// <param name="classNames">The class names for the desired visitor instances.</param>
    public IContextVisitor[] CreateMany(string classNames)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }
  }
}

