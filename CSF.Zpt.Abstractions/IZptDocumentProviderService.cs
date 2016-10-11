using System;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a service which provides access to the various registered <see cref="IZptDocumentProvider"/>
  /// instances.
  /// </summary>
  public interface IZptDocumentProviderService
  {
    /// <summary>
    /// Gets a provider based upon its <c>System.Type</c>.
    /// </summary>
    /// <returns>The provider.</returns>
    /// <param name="providerType">Provider type.</param>
    IZptDocumentProvider GetProvider(Type providerType);

    /// <summary>
    /// Gets a provider based upon its <c>System.Type</c> assembly-qualified name.
    /// </summary>
    /// <returns>The provider.</returns>
    /// <param name="providerTypeName">Provider type name.</param>
    IZptDocumentProvider GetProvider(string providerTypeName);

    /// <summary>
    /// Gets the default provider for HTML documents.
    /// </summary>
    /// <returns>The default HTML provider.</returns>
    IZptDocumentProvider GetDefaultHtmlProvider();

    /// <summary>
    /// Gets the default provider for XML documents.
    /// </summary>
    /// <returns>The default XML provider.</returns>
    IZptDocumentProvider GetDefaultXmlProvider();
  }
}

