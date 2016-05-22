using System;
using System.Linq;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Type which creates instances of <see cref="IContextVisitor"/> via reflection.
  /// </summary>
  public class ContextVisitorFactory : IContextVisitorFactory
  {
    #region constants

    private const char SEPARATOR = ';';

    #endregion

    #region methods

    /// <summary>
    /// Create the visitor from a fully-qualified type name.
    /// </summary>
    /// <param name="className">The class name for the desired visitor instance.</param>
    public IContextVisitor Create(string className)
    {
      IContextVisitor output;
      Type outputType;

      if(String.IsNullOrEmpty(className))
      {
        output = null;
      }
      else if((outputType = Type.GetType(className)) != null
              && typeof(IContextVisitor).IsAssignableFrom(outputType))
      {
        output = (IContextVisitor) Activator.CreateInstance(outputType);
      }
      else
      {
        output = null;
      }

      return output;
    }

    /// <summary>
    /// Create a collection of visitors from a semicolon-separated list of fully-qualified type name.
    /// </summary>
    /// <param name="classNames">The class names for the desired visitor instances.</param>
    public IContextVisitor[] CreateMany(string classNames)
    {
      IContextVisitor[] output;

      if(!String.IsNullOrEmpty(classNames))
      {
        output = classNames
          .Split(SEPARATOR)
          .Select(x => Create(x))
          .Where(x => x != null)
          .ToArray();
      }
      else
      {
        output = null;
      }

      return output;
    }

    #endregion
  }
}

