using System;
using System.Collections.Generic;
using CSF.Reflection;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Provides a forwards-only reader for <see cref="Path"/> instances, behaving similarly to an <c>IEnumerator</c>.
  /// </summary>
  public class PathWalker
  {
    #region fields

    private Path _path;
    private IEnumerator<PathComponent> _componentEnumerator;
    private IEnumerator<PathPart> _partEnumerator;

    #endregion

    #region properties

    /// <summary>
    /// Gets the source TALES Path.
    /// </summary>
    /// <value>The path.</value>
    public Path Path
    {
      get {
        return _path;
      }
    }

    /// <summary>
    /// Gets the current <see cref="PathComponent"/>.
    /// </summary>
    /// <value>The current path component.</value>
    public PathComponent CurrentComponent
    {
      get {
        return (_componentEnumerator != null)? _componentEnumerator.Current : null;
      }
    }

    /// <summary>
    /// Gets the current <see cref="PathPart"/>.
    /// </summary>
    /// <value>The current path part.</value>
    public PathPart CurrentPart
    {
      get {
        return (_partEnumerator != null)? _partEnumerator.Current : null;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Moves the current instance to the next path component.
    /// </summary>
    /// <returns><c>true</c>, if there was a 'next' path component to move to, <c>false</c> otherwise.</returns>
    public bool NextComponent()
    {
      if(_componentEnumerator == null)
      {
        _componentEnumerator = _path.Components.GetEnumerator();
      }

      _partEnumerator = null;
      return _componentEnumerator.MoveNext();
    }

    /// <summary>
    /// Moves the current instance to the next path part.
    /// </summary>
    /// <returns><c>true</c>, if there was a 'next' path part to move to, <c>false</c> otherwise.</returns>
    public bool NextPart()
    {
      if(_componentEnumerator == null
         || _componentEnumerator.Current == null)
      {
        string message = String.Format(Resources.ExceptionMessages.NoCurrentComponentToWalk,
                                       typeof(PathComponent).Name,
                                       Reflect.Method<PathWalker>(x => x.NextComponent()).Name);
        throw new InvalidOperationException(message);
      }

      if(_partEnumerator == null)
      {
        _partEnumerator = _componentEnumerator.Current.Parts.GetEnumerator();
      }

      return _partEnumerator.MoveNext();
    }

    /// <summary>
    /// Resets the state of this instance, back to one item before the first component and the first part.
    /// </summary>
    public void Reset()
    {
      _componentEnumerator.Reset();
      _partEnumerator = null;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.PathWalker"/> class.
    /// </summary>
    /// <param name="path">Path.</param>
    public PathWalker(Path path)
    {
      if(path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      _path = path;
    }

    #endregion
  }
}

