using System;

namespace CSF.Zpt
{
  public class ZptDocumentProviderMetadata
  {
    #region properties

    public Type ProviderType
    {
      get;
      private set;
    }

    public bool IsDefaultHtmlProvider
    {
      get;
      private set;
    }

    public bool IsDefaultXmlProvider
    {
      get;
      private set;
    }

    #endregion

    #region constructor

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

