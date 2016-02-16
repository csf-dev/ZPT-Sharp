﻿using System;
using System.IO;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Provides information about the source file from which an <see cref="ZptElement"/> is derived.
  /// </summary>
  public class SourceFileInfo : IEquatable<SourceFileInfo>
  {
    #region fields

    private FileInfo _osFile;
    private string _abstractPath;

    #endregion

    #region properties

    /// <summary>
    /// Gets a reference to the operating system file represented by the current instance.
    /// </summary>
    /// <value>The file info.</value>
    public FileInfo FileInfo
    {
      get {
        return _osFile;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the filename of the current source file.
    /// </summary>
    /// <returns>The filename.</returns>
    public virtual string GetFullName()
    {
      return (this.FileInfo != null)? this.FileInfo.FullName : _abstractPath;
    }

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
        if(this.FileInfo != null)
        {
          output = obj.FileInfo == this.FileInfo;
        }
        else
        {
          output = obj._abstractPath == _abstractPath;
        }
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
      return (this.FileInfo != null)? this.FileInfo.GetHashCode() : _abstractPath.GetHashCode();
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> class.
    /// </summary>
    protected SourceFileInfo() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> class.
    /// </summary>
    /// <param name="fileInfo">File info.</param>
    public SourceFileInfo(FileInfo fileInfo)
    {
      if(fileInfo == null)
      {
        throw new ArgumentNullException("fileInfo");
      }

      _osFile = fileInfo;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.SourceFileInfo"/> class.
    /// </summary>
    /// <param name="abstractPath">Abstract path.</param>
    public SourceFileInfo(string abstractPath)
    {
      if(abstractPath == null)
      {
        throw new ArgumentNullException("abstractPath");
      }

      _abstractPath = abstractPath;
    }

    #endregion
  }
}
