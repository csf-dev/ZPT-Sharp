using System.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Interface for a type which enables selection of the output options for <see cref="IBatchRenderingOptions"/>, when
  /// the input options indicate a single document.
  /// </summary>
  public interface IBatchOptionsSingleInputOutputHelper
  {
    /// <summary>
    /// Indicates that the output is to a <c>System.IO.Stream</c>.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="stream">The stream.</param>
    IBatchRenderingOptions ToStream(Stream stream);

    /// <summary>
    /// Indicates that the output is to a single file.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="path">The file path.</param>
    IBatchRenderingOptions ToFile(string path);

    /// <summary>
    /// Indicates that the output is to a single file.
    /// </summary>
    /// <returns>The rendering options.</returns>
    /// <param name="file">The file.</param>
    IBatchRenderingOptions ToFile(FileInfo file);
  }
}