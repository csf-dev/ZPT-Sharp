using System;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

namespace CSF.Zpt.Rendering
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
    /// <returns><c>true</c> if the path traversal was a success; <c>false</c> otherwise.</returns>
    /// <param name="pathFragment">The path fragment.</param>
    /// <param name="result">Exposes the result if the traversal was a success</param>
    public bool HandleTalesPath(string pathFragment, out object result)
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

