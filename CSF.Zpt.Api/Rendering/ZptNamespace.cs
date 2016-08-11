using System;

namespace CSF.Zpt.Rendering
{
  /// <summary>
  /// Represents a ZPT namespace, by either or both of its prefix and namespace URI.
  /// </summary>
  public class ZptNamespace : IEquatable<ZptNamespace>
  {
    #region fields

    private static readonly ZptNamespace _default;

    #endregion

    #region properties

    /// <summary>
    /// Gets the full namespace URI.
    /// </summary>
    /// <value>The namespace URI.</value>
    public string Uri
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the namespace prefix.
    /// </summary>
    /// <value>The namespace prefix.</value>
    public string Prefix
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Determines whether the specified <see cref="CSF.Zpt.Rendering.ZptNamespace"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptNamespace"/>.
    /// </summary>
    /// <param name="other">
    /// The <see cref="CSF.Zpt.Rendering.ZptNamespace"/> to compare with the current
    /// <see cref="CSF.Zpt.Rendering.ZptNamespace"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="CSF.Zpt.Rendering.ZptNamespace"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptNamespace"/>; otherwise, <c>false</c>.
    /// </returns>
    public bool Equals(ZptNamespace other)
    {
      bool output;

      if(Object.ReferenceEquals(this, other))
      {
        output = true;
      }
      else if((object) other == null)
      {
        output = false;
      }
      else if(this.Uri != null)
      {
        output = this.Uri == other.Uri;
      }
      else
      {
        output = (other.Uri == null
                  && this.Prefix == other.Prefix);
      }

      return output;
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptNamespace"/>.
    /// </summary>
    /// <param name="obj">
    /// The <see cref="System.Object"/> to compare with the current <see cref="CSF.Zpt.Rendering.ZptNamespace"/>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current
    /// <see cref="CSF.Zpt.Rendering.ZptNamespace"/>; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals(object obj)
    {
      return this.Equals(obj as ZptNamespace);
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="CSF.Zpt.Rendering.ZptNamespace"/> object.
    /// </summary>
    /// <returns>
    /// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
    /// hash table.
    /// </returns>
    public override int GetHashCode()
    {
      string
        hashableUri = this.Uri?? String.Empty,
        hashablePrefix = this.Prefix?? String.Empty;

      return hashableUri.GetHashCode() ^ hashablePrefix.GetHashCode();
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Rendering.ZptNamespace"/>.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String"/> that represents the current <see cref="CSF.Zpt.Rendering.ZptNamespace"/>.
    /// </returns>
    public override string ToString()
    {
      string output;

      if(this.Prefix == null
         && this.Uri == null)
      {
        output = String.Empty;
      }
      else if(this.Prefix != null
              && this.Uri != null)
      {
        output = String.Format("[({0}):({1})]", this.Prefix, this.Uri);
      }
      else if(this.Prefix != null)
      {
        output = this.Prefix;
      }
      else
      {
        output = this.Uri;
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Rendering.ZptNamespace"/> class.
    /// </summary>
    /// <param name="prefix">The namespace prefix.</param>
    /// <param name="uri">The full namespace URI.</param>
    public ZptNamespace(string prefix = null,
                        string uri = null)
    {
      this.Uri = String.IsNullOrEmpty(uri)? null : uri;
      this.Prefix = String.IsNullOrEmpty(prefix)? null : prefix;
    }

    /// <summary>
    /// Initializes the <see cref="CSF.Zpt.Rendering.ZptNamespace"/> class.
    /// </summary>
    static ZptNamespace()
    {
      _default = new ZptNamespace(null, null);
    }

    #endregion

    #region static properties

    /// <summary>
    /// Gets an instance representing a document's default namespace (IE: no namespace).
    /// </summary>
    /// <value>The default namespace.</value>
    public static ZptNamespace Default
    {
      get {
        return _default;
      }
    }

    #endregion

    #region operator overloads

    /// <summary>
    /// Operator overload for testing equality between two <see cref="ZptNamespace"/> instances.
    /// </summary>
    /// <param name="first">The first instance.</param>
    /// <param name="second">The second instance.</param>
    public static bool operator ==(ZptNamespace first, ZptNamespace second)
    {
      bool output;

      if(Object.ReferenceEquals(first, second))
      {
        output = true;
      }
      else if((object) first == null)
      {
        output = false;
      }
      else
      {
        output = first.Equals(second);
      }

      return output;
    }

    /// <summary>
    /// Operator overload for testing inequality between two <see cref="ZptNamespace"/> instances.
    /// </summary>
    /// <param name="first">The first instance.</param>
    /// <param name="second">The second instance.</param>
    public static bool operator !=(ZptNamespace first, ZptNamespace second)
    {
      return !(first == second);
    }

    #endregion
  }
}

