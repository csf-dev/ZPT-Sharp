using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Implementation of <see cref="ISourceInfo"/> representing an abstract string path.
  /// </summary>
  public class AbstractSourceInfo : ISourceInfo
  {
    #region fields

    private string _abstractPath;

    #endregion

    #region properties

    /// <summary>
    /// Gets the full name of the current source file.
    /// </summary>
    /// <returns>The name.</returns>
    public virtual string FullName
    {
      get {
        return _abstractPath;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="obj">
    /// The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>.
    /// </param>
    public override bool Equals(object obj)
    {
      return this.Equals(obj as AbstractSourceInfo);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.ISourceInfo"/> is equal to the current <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>.
    /// </summary>
    /// <param name="obj">The <see cref="CSF.Zpt.Rendering.ISourceInfo"/> to compare with the current <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ISourceInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>; otherwise, <c>false</c>.</returns>
    public virtual bool Equals(ISourceInfo obj)
    {
      return this.Equals(obj as AbstractSourceInfo);
    }

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>; otherwise, <c>false</c>.
    /// </returns>
    /// <param name="obj">
    /// The <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/>.
    /// </param>
    public virtual bool Equals(AbstractSourceInfo obj)
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
        output = obj.FullName == this.FullName;
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
      return this.FullName.GetHashCode();
    }

    /// <summary>
    /// Gets a representation of the current instance which is suitable for use with TALES.
    /// </summary>
    /// <returns>The TALES representation of the current instance.</returns>
    public object GetContainer()
    {
      return null;
    }

    /// <summary>
    /// Gets a name for the current instance, relative to a given root name.  The meaning of relative is up to the
    /// implementation.
    /// </summary>
    /// <returns>The relative name.</returns>
    /// <param name="root">The root name.</param>
    public string GetRelativeName(string root)
    {
      return FullName;
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.AbstractSourceInfo"/> class.
    /// </summary>
    /// <param name="abstractPath">Abstract path.</param>
    public AbstractSourceInfo(string abstractPath)
    {
      if(abstractPath == null)
      {
        throw new ArgumentNullException(nameof(abstractPath));
      }

      _abstractPath = abstractPath;
    }

    #endregion
  }
}

