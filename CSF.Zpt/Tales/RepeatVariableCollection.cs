using System;

namespace CSF.Zpt.Tales
{
  /// <summary>
  /// Represents a collection exposing information about the <c>repeat</c> operations which are currently in effect.
  /// </summary>
  public class RepeatVariableCollection : ITalesPathHandler
  {
    #region methods

    /// <summary>
    /// Gets a specific piece of repeat information by name (a TALES path fragment).
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

