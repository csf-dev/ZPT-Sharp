using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Zpt.ExpressionEvaluators
{
  /// <summary>
  /// Represents a TALES path, which is composed of one or more <see cref="PathComponent"/>, each of which is
  /// in turn composed of one or more <see cref="PathPart"/>.
  /// </summary>
  public class Path
  {
    #region constants

    private const char
      COMPOSITE_PATHS_SEPARATOR = '|',
      PATH_SEPARATOR            = '/';

    #endregion

    #region fields

    private ReadOnlyCollection<PathComponent> _components;

    #endregion

    #region properties

    /// <summary>
    /// Gets a collection of the <see cref="PathComponent"/> which comprise the current instance.
    /// </summary>
    /// <value>The path components.</value>
    public IReadOnlyList<PathComponent> Components
    {
      get {
        return _components;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ExpressionEvaluators.Path"/> class.
    /// </summary>
    /// <param name="components">Components.</param>
    internal Path(PathComponent[] components)
    {
      if(components == null)
      {
        throw new ArgumentNullException(nameof(components));
      }

      _components = new ReadOnlyCollection<PathComponent>(components);
    }

    #endregion

    #region static methods

    /// <summary>
    /// Creates and returns a new <see cref="Path"/> object model, based on the given <c>System.String</c> path
    /// expression.
    /// </summary>
    /// <param name="pathExpression">The path expression.</param>
    public static Path Create(string pathExpression)
    {
      if(pathExpression == null)
      {
        throw new ArgumentNullException(nameof(pathExpression));
      }

      var componentStrings = pathExpression
        .Split(COMPOSITE_PATHS_SEPARATOR)
        .Select(x => x.Trim());
      
      var components = (from component in componentStrings
                        let parts = component.Split(PATH_SEPARATOR)
                        select new PathComponent(parts.Select(x => new PathPart(x)).ToArray()))
        .ToArray();
      
      return new Path(components);
    }

    #endregion
  }
}

