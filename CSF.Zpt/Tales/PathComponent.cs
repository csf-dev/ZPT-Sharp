using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents a component of a composite TALES path.  This type is comprised of one or more <see cref="PathPart"/>.
  /// </summary>
  public class PathComponent
  {
    #region fields

    private ReadOnlyCollection<PathPart> _parts;

    #endregion

    #region properties

    /// <summary>
    /// Gets a collection of the <see cref="PathPart"/> which comprise the current instance.
    /// </summary>
    /// <value>The path parts.</value>
    public IReadOnlyList<PathPart> Parts
    {
      get {
        return _parts;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Tales.PathComponent"/> class.
    /// </summary>
    /// <param name="parts">Parts.</param>
    internal PathComponent(PathPart[] parts)
    {
      if(parts == null)
      {
        throw new ArgumentNullException("parts");
      }

      _parts = new ReadOnlyCollection<PathPart>(parts);
    }

    #endregion
  }
}

