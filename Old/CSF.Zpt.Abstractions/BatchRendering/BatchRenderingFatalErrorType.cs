namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Enumerates the expected fatal error types which can occur whilst rendering a batch of documents.
  /// </summary>
  public enum BatchRenderingFatalErrorType
  {
    /// <summary>
    /// Indicates that no input paths or streams were specified, thus the renderer has no work to do.
    /// </summary>
    NoInputsSpecified = 1,

    /// <summary>
    /// The renderer does not support rendering from both streams and paths.  This indicates that both were provided.
    /// </summary>
    InputCannotBeBothStreamAndPaths,

    /// <summary>
    /// Indicates that no output mechanism was specified (either s filesystem path or a stream).
    /// </summary>
    NoOutputsSpecified,

    /// <summary>
    /// The renderer does not support rendering to both streams and paths.  This indicates that both were provided.
    /// </summary>
    OutputCannotBeBothStreamAndPaths,
  }
}