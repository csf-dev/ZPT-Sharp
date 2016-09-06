using System;
using System.Collections.Generic;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Represents batch rendering options, indicating the inputs and outputs for a <see cref="IBatchRenderer"/>.
  /// </summary>
  public interface IBatchRenderingOptions
  {
    #region input options

    /// <summary>
    /// Gets the input stream, or a <c>null</c> reference if it is unset.
    /// </summary>
    /// <value>The input stream.</value>
    Stream InputStream { get; }

    /// <summary>
    /// Gets the input paths, which may be an empty set if it is unset.
    /// </summary>
    /// <value>The input paths.</value>
    IEnumerable<FileSystemInfo> InputPaths { get; }

    /// <summary>
    /// Gets a collection of ignored paths, which shall be excluded when examining the <see cref="InputPaths"/>.
    /// </summary>
    /// <value>The ignored paths.</value>
    IEnumerable<DirectoryInfo> IgnoredPaths { get; }

    /// <summary>
    /// Gets a glob search pattern for examining the <see cref="InputPaths"/>.
    /// </summary>
    /// <value>The input search pattern.</value>
    string InputSearchPattern { get; }

    #endregion

    #region output options

    /// <summary>
    /// Gets the output stream, or a <c>null</c> reference if it is unset.
    /// </summary>
    /// <value>The output stream.</value>
    Stream OutputStream { get; }

    /// <summary>
    /// Gets the output path, or a <c>null</c> reference if it is unset.
    /// </summary>
    /// <value>The output path.</value>
    FileSystemInfo OutputPath { get; }

    /// <summary>
    /// Gets an optional override for setting the file extension of written output files.
    /// </summary>
    /// <value>The output extension override.</value>
    string OutputExtensionOverride { get; }

    #endregion

    #region other options

    /// <summary>
    /// Gets the error handling strategy for the batch renderer.
    /// </summary>
    /// <value>The error handling strategy.</value>
    BatchErrorHandlingStrategy ErrorHandlingStrategy { get; }

    #endregion
  }
}

