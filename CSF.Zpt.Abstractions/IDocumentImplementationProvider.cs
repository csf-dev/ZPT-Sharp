using System;
using System.Collections.Generic;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a type which provides metadata about the available concrete implementations of
  /// <see cref="IZptDocumentProvider"/>.
  /// </summary>
  public interface IDocumentImplementationProvider
  {
    /// <summary>
    /// Gets the metadata about all of the available providers.
    /// </summary>
    /// <returns>A collection of provider metadata instances.</returns>
    IEnumerable<ZptDocumentProviderMetadata> GetAllProviderMetadata();
  }
}

