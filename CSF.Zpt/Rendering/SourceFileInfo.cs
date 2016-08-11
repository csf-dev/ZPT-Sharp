using System;
using System.IO;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Provides information about the source file from which an <see cref="ZptElement"/> is derived.
  /// </summary>
  public class SourceFileInfo : ISourceInfo
  {
    #region fields

    private FileInfo _osFile;

    #endregion

    #region properties

    /// <summary>
    /// Gets the filename of the current source file.
    /// </summary>
    /// <returns>The filename.</returns>
    public virtual string FullName
    {
      get {
        return this.FileInfo.FullName;
      }
    }

    /// <summary>
    /// Gets a reference to the operating system file represented by the current instance.
    /// </summary>
    /// <value>The file info.</value>
    public virtual FileInfo FileInfo
    {
      get {
        return _osFile;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="obj">
    /// The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>.
    /// </param>
    public override bool Equals(object obj)
    {
      return this.Equals(obj as SourceFileInfo);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.ISourceInfo"/> is equal to the current <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>.
    /// </summary>
    /// <param name="obj">The <see cref="CSF.Zpt.Rendering.ISourceInfo"/> to compare with the current <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ISourceInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(ISourceInfo obj)
    {
      return this.Equals(obj as SourceFileInfo);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="obj">
    /// The <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.SourceFileInfo"/>.
    /// </param>
    public virtual bool Equals(SourceFileInfo obj)
    {
      bool output;

      if(Object.ReferenceEquals(this, obj))
      {
        output = true;
      }
      else if(obj == null)
      {
        output = false;
      }
      else
      {
        output = obj.FileInfo == this.FileInfo;
      }

      return output;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public override int GetHashCode()
    {
      return this.FileInfo.GetHashCode();
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> class.
    /// </summary>
    /// <param name="fileInfo">File info.</param>
    public SourceFileInfo(FileInfo fileInfo)
    {
      if(fileInfo == null)
      {
        throw new ArgumentNullException(nameof(fileInfo));
      }

      _osFile = fileInfo;
    }

    #endregion
  }
}

