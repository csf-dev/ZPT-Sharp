using System;

namespace CSF.Zpt
{
  public interface IZptDocumentProviderRegistry
  {
    IZptDocumentProvider DefaultHtml { get; }

    IZptDocumentProvider DefaultXml { get; }

    IZptDocumentProvider Get(string typeName);
  }
}

