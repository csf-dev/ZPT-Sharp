using System;
using System.Collections.Generic;

namespace CSF.Zpt
{
  public interface IDocumentImplementationProvider
  {
    IEnumerable<ZptDocumentProviderMetadata> GetAllProviderMetadata();
  }
}

