using System;
using System.Collections.Generic;
using System.IO;
using CSF.IO;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using System.Linq;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Default implementation of <see cref="IBatchRenderer"/>.
  /// </summary>
  public class BatchRenderer : IBatchRenderer
  {
    #region fields

    private readonly IRenderingJobFactory _jobFactory;

    #endregion

    #region public API

    /// <summary>
    /// Parse and render the documents found using the given batch rendering options.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    /// <param name="batchOptions">Batch rendering options, indicating the source and destination files.</param>
    /// <param name="mode">An optional override for the rendering mode.</param>
    public virtual IBatchRenderingResponse Render(IRenderingOptions options,
                                                  IBatchRenderingOptions batchOptions,
                                                  RenderingMode? mode)
    {
      ValidateBatchOptions(batchOptions);

      var jobs = GetRenderingJobs(batchOptions, mode);

      List<IBatchRenderingDocumentResponse> documents = new List<IBatchRenderingDocumentResponse>();

      foreach(var job in jobs)
      {
        var contextConfigurator = GetContextConfigurator(job);

        var docResponse = Render(job, options, batchOptions, contextConfigurator);
        documents.Add(docResponse);
      }

      return new BatchRenderingResponse(documents);
    }

    #endregion

    #region protected methods

    protected virtual IBatchRenderingDocumentResponse Render(IRenderingJob job,
                                                             IRenderingOptions options,
                                                             IBatchRenderingOptions batchOptions,
                                                             Action<RenderingContext> contextConfigurator)
    {
      using(var outputStream = job.GetOutputStream(batchOptions))
      using(var writer = new StreamWriter(outputStream, options.OutputEncoding))
      {
        job.Document.Render(writer,
                            options: options,
                            contextConfigurator: contextConfigurator);
      }

      return new BatchRenderingDocumentResponse(job.Document.GetSourceInfo(),
                                                job.GetOutputInfo(batchOptions));
    }

    protected virtual IEnumerable<IRenderingJob> GetRenderingJobs(IBatchRenderingOptions options, RenderingMode? mode)
    {
      return _jobFactory.GetRenderingJobs(options, mode);
    }

    protected virtual Action<RenderingContext> GetContextConfigurator(IRenderingJob job)
    {
      return ctx => {
        if(job.InputRootDirectory != null)
        {
          var docRoot = new TemplateDirectory(job.InputRootDirectory);
          ctx.MetalModel.AddGlobal("documents", docRoot);
        }
      };
    }

    protected virtual void ValidateBatchOptions(IBatchRenderingOptions options)
    {
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

    #endregion

    #region constructor

    public BatchRenderer(IRenderingJobFactory renderingJobFactory = null)
    {
      _jobFactory = renderingJobFactory?? new RenderingJobFactory();
    }

    #endregion
  }
}

