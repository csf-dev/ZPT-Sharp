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
    IZptDocumentProvider DefaultHtml { get; }

    /// <summary>
    /// Gets the default XML ZPT document provider.
    /// </summary>
    /// <value>The default XML document provider.</value>
    IZptDocumentProvider DefaultXml { get; }

    /// <summary>
    /// Gets an <see cref="IZptDocumentProvider"/> by its assembly-qualified type name.
    /// </summary>
    /// <param name="typeName">The assembly-qualified name.</param>
    IZptDocumentProvider Get(string typeName);
  }
}

