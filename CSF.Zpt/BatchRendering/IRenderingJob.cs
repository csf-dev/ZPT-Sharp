using System;
using System.IO;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Represents a task to render a single <see cref="IZptDocument"/>.
  /// </summary>
  public interface IRenderingJob
  {
    /// <summary>
    /// Gets the root directory for input documents.
    /// </summary>
    /// <value>The input root directory.</value>
    DirectoryInfo InputRootDirectory { get; }

    /// <summary>
    /// Gets the ZPT document.
    /// </summary>
    /// <returns>The document.</returns>
    IZptDocument GetDocument();

    /// <summary>
    /// Gets a <c>System.String</c> representing information about the output location.
    /// </summary>
    /// <returns>The output info.</returns>
    /// <param name="batchOptions">Batch options.</param>
    string GetOutputInfo(IBatchRenderingOptions batchOptions);

    /// <summary>
    /// Gets the output stream to which the rendered document will be output.
    /// </summary>
    /// <returns>The output stream.</returns>
    /// <param name="batchOptions">Batch options.</param>
    Stream GetOutputStream(IBatchRenderingOptions batchOptions);
  }
}

