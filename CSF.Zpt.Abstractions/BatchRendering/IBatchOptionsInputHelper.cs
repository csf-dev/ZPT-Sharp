using System.Collections.Generic;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Interface for a type which enables selection of the input options for <see cref="IBatchRenderingOptions"/>.
  /// </summary>
  public interface IBatchOptionsInputHelper
  {
    /// <summary>
    /// Specifies that the batch options are to be built from a <c>System.IO.Stream</c>.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="stream">The stream.</param>
    /// <param name="mode">The rendering mode.</param>
    IBatchOptionsSingleInputOutputHelper FromStream(Stream stream, RenderingMode mode);

    /// <summary>
    /// Specifies that the batch options are to be built from a single input file.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="path">The file path.</param>
    /// <param name="mode">An optional rendering mode.</param>
    IBatchOptionsSingleInputOutputHelper FromFile(string path, RenderingMode? mode);

    /// <summary>
    /// Specifies that the batch options are to be built from a single input file.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="file">The file.</param>
    /// <param name="mode">An optional rendering mode.</param>
    IBatchOptionsSingleInputOutputHelper FromFile(FileInfo file, RenderingMode? mode);

    /// <summary>
    /// Specifies that the batch options are to be built from a collection of input files.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="paths">The file paths.</param>
    /// <param name="mode">An optional rendering mode.</param>
    IBatchOptionsMultipleInputOutputHelper FromFiles(IEnumerable<string> paths, RenderingMode? mode);

    /// <summary>
    /// Specifies that the batch options are to be built from a collection of input files.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="files">The files.</param>
    /// <param name="mode">An optional rendering mode.</param>
    IBatchOptionsMultipleInputOutputHelper FromFiles(IEnumerable<FileInfo> files, RenderingMode? mode);

    /// <summary>
    /// Specifies that the batch options are to be built from the files found within a directory path.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="path">The directory path.</param>
    /// <param name="mode">An optional rendering mode.</param>
    /// <param name="ignoredPaths">An optional collection of ignored paths.</param>
    /// <param name="searchPattern">An optional search pattern for files within the directory.</param>
    IBatchOptionsMultipleInputOutputHelper FromDirectory(string path, RenderingMode? mode, IEnumerable<string> ignoredPaths = null, string searchPattern = null);

    /// <summary>
    /// Specifies that the batch options are to be built from the files found within a directory path.
    /// </summary>
    /// <returns>The output options helper.</returns>
    /// <param name="directory">The directory.</param>
    /// <param name="mode">An optional rendering mode.</param>
    /// <param name="ignoredPaths">An optional collection of ignored directories.</param>
    /// <param name="searchPattern">An optional search pattern for files within the directory.</param>
    IBatchOptionsMultipleInputOutputHelper FromDirectory(DirectoryInfo directory, RenderingMode? mode, IEnumerable<DirectoryInfo> ignoredPaths = null, string searchPattern = null);
  }
}