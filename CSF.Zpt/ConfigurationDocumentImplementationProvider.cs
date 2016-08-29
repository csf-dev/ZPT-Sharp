using System;
using System.Collections.Generic;
using CSF.Configuration;
using System.Linq;

namespace CSF.Zpt
{
  /// <summary>
  /// Implementation of <see cref="IDocumentImplementationProvider"/> which makes use of the configuration file to
  /// discover the available document implementations.
  /// </summary>
  public class ConfigurationDocumentImplementationProvider : IDocumentImplementationProvider
  {
    #region fields

    private static DocumentImplementationConfiguration _config;

    #endregion

    #region methods

    /// <summary>
    /// Gets the metadata about all of the available providers.
    /// </summary>
    /// <returns>A collection of provider metadata instances.</returns>
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

