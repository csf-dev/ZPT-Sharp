using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Represents metadata about an implementation of <see cref="IZptDocumentProvider"/>.
  /// </summary>
  public class ZptDocumentProviderMetadata
  {
    #region properties

    /// <summary>
    /// Gets the <c>System.Type</c> of the provider implementation.
    /// </summary>
    /// <value>The provider type.</value>
    public Type ProviderType
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is the default provider for HTML documents.
    /// </summary>
    /// <value><c>true</c> if this instance is the default HTML provider; otherwise, <c>false</c>.</value>
    public bool IsDefaultHtmlProvider
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is the default provider for XML documents.
    /// </summary>
    /// <value><c>true</c> if this instance is the default XML provider; otherwise, <c>false</c>.</value>
    public bool IsDefaultXmlProvider
    {
      get;
      private set;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.ZptDocumentProviderMetadata"/> class.
    /// </summary>
    /// <param name="type">The provider type.</param>
    /// <param name="defaultHtml">If set to <c>true</c> then this is the default HTML provider.</param>
    /// <param name="defaultXMl">If set to <c>true</c> this is the default XML provider.</param>
    public ZptDocumentProviderMetadata(Type type, bool defaultHtml = false, bool defaultXMl = false)
    {
      if(type == null)
      {
        throw new ArgumentNullException(nameof(type));
      }

      this.ProviderType = type;
      this.IsDefaultHtmlProvider = defaultHtml;
      this.IsDefaultXmlProvider = defaultXMl;
    }

    #endregion
  }
}

