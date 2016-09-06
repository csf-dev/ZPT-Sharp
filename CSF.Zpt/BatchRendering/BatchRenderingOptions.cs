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

    #region other options

    /// <summary>
    /// Gets the error handling strategy for the batch renderer.
    /// </summary>
    /// <value>The error handling strategy.</value>
    public BatchErrorHandlingStrategy ErrorHandlingStrategy { get; private set; }

    #endregion

    #region methods

    /// <summary>
    /// Validates the state of the current instance, raising an exception if it is not valid.
    /// </summary>
    private void Validate()
    {
      if(this.InputStream == null && !this.InputPaths.Any())
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustHaveInputStreamOrPaths;
        throw new InvalidBatchRenderingOptionsException(message);
      }
      else if(this.InputStream != null && this.InputPaths.Any())
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustNotHaveBothInputStreamAndPaths;
        throw new InvalidBatchRenderingOptionsException(message);
      }

      if(this.OutputStream == null && this.OutputPath == null)
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustHaveOutputStreamOrPath;
        throw new InvalidBatchRenderingOptionsException(message);
      }
      else if(this.OutputStream != null && this.OutputPath != null)
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustNotHaveBothOutputStreamAndPath;
        throw new InvalidBatchRenderingOptionsException(message);
      }

      if(!this.ErrorHandlingStrategy.IsDefinedValue())
      {
        string message = "TODO";
        throw new InvalidBatchRenderingOptionsException(message);
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
    /// <param name="errorHandling">The error handling strategy.</param>
    public BatchRenderingOptions (Stream inputStream = null,
                                  Stream outputStream = null,
                                  IEnumerable<FileSystemInfo> inputPaths = null,
                                  FileSystemInfo outputPath = null,
                                  string inputSearchPattern = null,
                                  string outputExtensionOverride = null,
                                  IEnumerable<DirectoryInfo> ignoredPaths = null,
                                  BatchErrorHandlingStrategy errorHandling = default(BatchErrorHandlingStrategy))
    {
      this.InputStream = inputStream;
      this.InputPaths = inputPaths?? new FileSystemInfo[0];
      this.IgnoredPaths = ignoredPaths;
      this.InputSearchPattern = inputSearchPattern?? DEFAULT_SEARCH_PATTERN;

      this.OutputStream = outputStream;
      this.OutputPath = outputPath;
      this.OutputExtensionOverride = outputExtensionOverride;

      this.ErrorHandlingStrategy = errorHandling;

      Validate();
    }

    #endregion
  }
}

