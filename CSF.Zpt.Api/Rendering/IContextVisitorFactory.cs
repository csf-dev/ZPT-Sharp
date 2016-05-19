using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which creates instances of <see cref="IContextVisitor"/> via reflection.
  /// </summary>
  public interface IContextVisitorFactory
  {
    /// <summary>
    /// Create the visitor from a fully-qualified type name.
    /// </summary>
    /// <param name="className">The class name for the desired visitor instance.</param>
    IContextVisitor Create(string className);

    /// <summary>
    /// Create a collection of visitors from a semicolon-separated list of fully-qualified type name.
    /// </summary>
    /// <param name="classNames">The class names for the desired visitor instances.</param>
    IContextVisitor[] CreateMany(string classNames);
  }
}

