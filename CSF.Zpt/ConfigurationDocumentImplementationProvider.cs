using System;
using System.Collections.Generic;
using CSF.Configuration;
using System.Linq;

namespace CSF.Zpt
{
  public class ConfigurationDocumentImplementationProvider : IDocumentImplementationProvider
  {
    #region fields

    private static DocumentImplementationConfiguration _config;

    #endregion

    #region methods

    public IEnumerable<ZptDocumentProviderMetadata> GetAllProviderMetadata()
    {
      return _config.Implementations
        .Cast<Implementation>()
        .Select(x => new ZptDocumentProviderMetadata(Type.GetType(x.TypeName), x.IsDefaultHtml, x.IsDefaultXml))
        .ToArray();
    }

    #endregion

    #region constructor

    static ConfigurationDocumentImplementationProvider()
    {
      _config = ConfigurationHelper.GetSection<DocumentImplementationConfiguration>();
    }

    #endregion
  }
}

