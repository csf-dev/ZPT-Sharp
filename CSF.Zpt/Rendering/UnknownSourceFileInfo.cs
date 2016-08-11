using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Null-object implementation of <see cref="SourceFileInfo"/>, representing an unknown source file.
  /// </summary>
  public class UnknownSourceFileInfo : ISourceInfo
  {
    #region constants

    private const string NAME = "[Unknown source file]";

    #endregion

    #region fields

    private static UnknownSourceFileInfo _default;

    #endregion

    #region overrides

    /// <summary>
    /// Gets the filename of the current source file.
    /// </summary>
    /// <returns>The filename.</returns>
    public string FullName
    {
      get {
        return NAME;
      }
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public int GetHashCode()
    {
      return 0;
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/>.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="System.Object"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/>; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(object obj)
    {
      return Object.ReferenceEquals(this, obj);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/>.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/>; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(ISourceInfo obj)
    {
      return Object.ReferenceEquals(this, obj);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/> class.
    /// </summary>
    private UnknownSourceFileInfo() {}

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Rendering.UnknownSourceFileInfo"/> class.
    /// </summary>
    static UnknownSourceFileInfo()
    {
      _default = new UnknownSourceFileInfo();
    }

    #endregion

    #region singleton

    /// <summary>
    /// Gets the default/singleton instance.
    /// </summary>
    /// <value>The instance.</value>
    public static ISourceInfo Instance { get { return _default; } }

    #endregion
  }
}

