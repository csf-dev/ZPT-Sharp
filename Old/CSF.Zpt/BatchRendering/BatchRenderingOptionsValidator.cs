using System;
using System.Linq;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Default implementation of <see cref="IBatchRenderingOptionsValidator"/>.
  /// </summary>
  public class BatchRenderingOptionsValidator : IBatchRenderingOptionsValidator
  {
    /// <summary>
    /// Validate the given options.
    /// </summary>
    /// <param name="options">The options to validate.</param>
    public void Validate(IBatchRenderingOptions options)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      if(options.InputStream == null && !options.InputPaths.Any())
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustHaveInputStreamOrPaths;
        throw new InvalidBatchRenderingOptionsException(message, BatchRenderingFatalErrorType.NoInputsSpecified);
      }
      else if(options.InputStream != null && options.InputPaths.Any())
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustNotHaveBothInputStreamAndPaths;
        throw new InvalidBatchRenderingOptionsException(message, BatchRenderingFatalErrorType.InputCannotBeBothStreamAndPaths);
      }

      if(options.OutputStream == null && options.OutputPath == null)
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustHaveOutputStreamOrPath;
        throw new InvalidBatchRenderingOptionsException(message, BatchRenderingFatalErrorType.NoOutputsSpecified);
      }
      else if(options.OutputStream != null && options.OutputPath != null)
      {
        string message = Resources.ExceptionMessages.BatchOptionsMustNotHaveBothOutputStreamAndPath;
        throw new InvalidBatchRenderingOptionsException(message, BatchRenderingFatalErrorType.OutputCannotBeBothStreamAndPaths);
      }
    }
  }
}

