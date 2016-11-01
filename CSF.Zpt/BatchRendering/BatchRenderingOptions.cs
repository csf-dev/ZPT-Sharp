using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Concrete implementation of <see cref="IBatchRenderingOptions"/>.
  /// </summary>
  public sealed class BatchRenderingOptions : IBatchRenderingOptions
  {
    #region constants

    private const string DEFAULT_SEARCH_PATTERN = "*";

    #endregion

    #region fields

    private bool _disposed;

    #endregion

    #region input options

    /// <summary>
    /// Gets the input stream, or a <c>null</c> reference if it is unset.
    /// </summary>
    /// <value>The input stream.</value>
    public Stream InputStream { get; private set; }

    /// <summary>
    /// Gets the input paths, which may be an empty set if it is unset.
    /// </summary>
    /// <value>The input paths.</value>
    public IEnumerable<FileSystemInfo> InputPaths { get; private set; }

    /// <summary>
    /// Gets a collection of ignored paths, which shall be excluded when examining the <see cref="InputPaths"/>.
    /// </summary>
    /// <value>The ignored paths.</value>
    public IEnumerable<DirectoryInfo> IgnoredPaths { get; private set; }

    /// <summary>
    /// Gets a glob search pattern for examining the <see cref="InputPaths"/>.
    /// </summary>
    /// <value>The input search pattern.</value>
    public string InputSearchPattern { get; private set; }

    /// <summary>
    /// Gets an optional value acting as an override for auto-detection of the rendering mode.
    /// </summary>
    /// <value>The rendering mode.</value>
    public RenderingMode? RenderingMode { get; private set; }

    #endregion

    #region output options

    /// <summary>
    /// Gets the output stream, or a <c>null</c> reference if it is unset.
    /// </summary>
    /// <value>The output stream.</value>
    public Stream OutputStream { get; private set; }

    /// <summary>
    /// Gets the output path, or a <c>null</c> reference if it is unset.
    /// </summary>
    /// <value>The output path.</value>
    public FileSystemInfo OutputPath { get; private set; }

    /// <summary>
    /// Gets an optional override for setting the file extension of written output files.
    /// </summary>
    /// <value>The output extension override.</value>
    public string OutputExtensionOverride { get; private set; }

    #endregion

    #region methods

    /// <summary>
    /// Releases all resource used by the <see cref="CSF.Zpt.BatchRendering.BatchRenderingOptions"/> object.
    /// </summary>
    /// <remarks>Call <see cref="Dispose"/> when you are finished using the
    /// <see cref="CSF.Zpt.BatchRendering.BatchRenderingOptions"/>. The <see cref="Dispose"/> method leaves the
    /// <see cref="CSF.Zpt.BatchRendering.BatchRenderingOptions"/> in an unusable state. After calling
    /// <see cref="Dispose"/>, you must release all references to the
    /// <see cref="CSF.Zpt.BatchRendering.BatchRenderingOptions"/> so the garbage collector can reclaim the memory that
    /// the <see cref="CSF.Zpt.BatchRendering.BatchRenderingOptions"/> was occupying.</remarks>
    public void Dispose()
    {
      if(!_disposed)
      {
        if(InputStream != null)
        {
          InputStream.Dispose();
        }

        if(OutputStream != null)
        {
          OutputStream.Dispose();
        }

        _disposed = true;
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderingOptions"/> class.
    /// </summary>
    /// <param name="inputStream">Input stream.</param>
    /// <param name="outputStream">Output stream.</param>
    /// <param name="inputPaths">Input paths.</param>
    /// <param name="outputPath">Output path.</param>
    /// <param name="inputSearchPattern">Input search pattern.</param>
    /// <param name="outputExtensionOverride">Output extension override.</param>
    /// <param name="ignoredPaths">Ignored paths.</param>
    /// <param name="renderingMode">The rendering mode override.</param>
    public BatchRenderingOptions (Stream inputStream = null,
                                  Stream outputStream = null,
                                  IEnumerable<FileSystemInfo> inputPaths = null,
                                  FileSystemInfo outputPath = null,
                                  string inputSearchPattern = null,
                                  string outputExtensionOverride = null,
                                  IEnumerable<DirectoryInfo> ignoredPaths = null,
                                  RenderingMode? renderingMode = null)
    {
      if(renderingMode.HasValue)
      {
        renderingMode.Value.RequireDefinedValue(nameof(renderingMode));
      }

      this.InputStream = inputStream;
      this.InputPaths = inputPaths?? new FileSystemInfo[0];
      this.IgnoredPaths = ignoredPaths;
      this.InputSearchPattern = inputSearchPattern?? DEFAULT_SEARCH_PATTERN;
      this.RenderingMode = renderingMode;

      this.OutputStream = outputStream;
      this.OutputPath = outputPath;
      this.OutputExtensionOverride = outputExtensionOverride;
    }

    #endregion

    #region static methods

    /// <summary>
    /// Create and return a 'builder' instance which helps construct valid instance of <see cref="IBatchRenderingOptions"/>.
    /// </summary>
    public static IBatchOptionsInputHelper Build()
    {
      return new BatchOptionsBuilder();
    }

    #endregion
  }
}

