using System;
using System.IO;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Provides information about the source file from which an <see cref="Element"/> is derived.
  /// </summary>
  public class SourceFileInfo
  {
    #region methods

    /// <summary>
    /// Gets a reference to the filesystem file from which the current file was drawn.
    /// </summary>
    /// <returns>The source file.</returns>
    public virtual FileInfo GetFileInfo()
    {
      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    #endregion
  }
}

