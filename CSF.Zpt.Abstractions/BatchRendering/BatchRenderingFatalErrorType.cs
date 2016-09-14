namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Enumerates the expected fatal error types which can occur whilst rendering a batch of documents.
  /// </summary>
  public enum BatchRenderingFatalErrorType
  {
    NoInputsSpecified = 1,

    InputCannotBeBothStreamAndPaths,

    NoOutputsSpecified,

    OutputCannotBeBothStreamAndPaths,
  }
}