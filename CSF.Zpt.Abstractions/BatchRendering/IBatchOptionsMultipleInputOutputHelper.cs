using System.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Interface for a type which enables selection of the output options for <see cref="IBatchRenderingOptions"/>, when
  /// the input options indicate the potential for multiple documents.
  /// </summary>
  public interface IBatchOptionsMultipleInputOutputHelper
  {
    /// <summary>
    /// Indicates that the output is to a filesystem directory.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="path">The directory path.</param>
    /// <param name="extensionOverride">An optional override for the file extension of rendered documents.</param>
    IBatchRenderingOptions ToDirectory(string path, string extensionOverride = null);

    /// <summary>
    /// Indicates that the output is to a filesystem directory.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="directory">The directory.</param>
    /// <param name="extensionOverride">An optional override for the file extension of rendered documents.</param>
    IBatchRenderingOptions ToDirectory(DirectoryInfo directory, string extensionOverride = null);
  }
}