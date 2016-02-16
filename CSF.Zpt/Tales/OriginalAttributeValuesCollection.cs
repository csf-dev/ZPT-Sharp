using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents a collection of <see cref="ZptAttribute"/>, exposing the original attributes present upon an element.
  /// </summary>
  public class OriginalAttributeValuesCollection : ITalesPathHandler
  {
    #region methods

    /// <summary>
    /// Gets a specific attribute by name (a TALES path fragment).
    /// </summary>
    /// <returns>The result of the path traversal.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    public object HandleTalesPath(string pathFragment)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

