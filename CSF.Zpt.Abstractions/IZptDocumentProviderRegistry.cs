using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a service which provides access to all of the <see cref="IZptDocumentProvider"/> instances
  /// registered in the current environment.
  /// </summary>
  public interface IZptDocumentProviderRegistry
  {
    /// <summary>
    /// Gets the default HTML ZPT document provider.
    /// </summary>
    /// <value>The default HTML document provider.</value>
    IZptDocumentProvider DefaultHtmlProvider { get; }

    /// <summary>
    /// Gets the default XML ZPT document provider.
    /// </summary>
    /// <value>The default XML document provider.</value>
    IZptDocumentProvider DefaultXmlProvider { get; }

    /// <summary>
    /// Gets an <see cref="IZptDocumentProvider"/> by its <c>System.Type</c>.
    /// </summary>
    /// <param name="type">The provider type.</param>
    IZptDocumentProvider GetProvider(Type type);
  }
}

