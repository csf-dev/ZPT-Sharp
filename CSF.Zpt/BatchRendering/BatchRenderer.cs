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

    /// <summary>
    /// Renders a single rendering job and returns a response.
    /// </summary>
    /// <param name="job">The job to render.</param>
    /// <param name="options">Rendering options.</param>
    /// <param name="batchOptions">Batch rendering options.</param>
    /// <param name="contextConfigurator">Context configurator.</param>
    protected virtual IBatchRenderingDocumentResponse Render(IRenderingJob job,
                                                             IRenderingOptions options,
                                                             IBatchRenderingOptions batchOptions,
                                                             Action<RenderingContext> contextConfigurator)
    {
      var doc = GetDocument(job);
      var outputInfo = GetOutputInfo(job, batchOptions);

      using(var outputStream = GetOutputStream(job, batchOptions))
      {
        return Render(doc, outputStream, options, contextConfigurator, outputInfo);
      }
    }

    /// <summary>
    /// Renders a single ZPT document and returns a response.
    /// </summary>
    /// <param name="doc">The document to render.</param>
    /// <param name="outputStream">The output stream.</param>
    /// <param name="options">Rendering options.</param>
    /// <param name="contextConfigurator">Context configurator.</param>
    /// <param name="outputInfo">Output info.</param>
    protected virtual IBatchRenderingDocumentResponse Render(IZptDocument doc,
                                                             Stream outputStream,
                                                             IRenderingOptions options,
                                                             Action<RenderingContext> contextConfigurator,
                                                             string outputInfo)
    {
      using(var writer = new StreamWriter(outputStream, options.OutputEncoding))
      {
        doc.Render(writer,
                   options: options,
                   contextConfigurator: contextConfigurator);
      }

      return new BatchRenderingDocumentResponse(doc.GetSourceInfo(), outputInfo);
    }

    /// <summary>
    /// Gets the rendering jobs from the rendering job factory.
    /// </summary>
    /// <returns>The rendering jobs.</returns>
    /// <param name="options">Batch rendering options.</param>
    /// <param name="mode">An optional rendering mode override.</param>
    protected virtual IEnumerable<IRenderingJob> GetRenderingJobs(IBatchRenderingOptions options, RenderingMode? mode)
    {
      return _jobFactory.GetRenderingJobs(options, mode);
    }

    /// <summary>
    /// Gets the rendering context configurator for a given job.
    /// </summary>
    /// <returns>The context configurator.</returns>
    /// <param name="job">Job.</param>
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

    /// <summary>
    /// Validates the batch rendering options.
    /// </summary>
    /// <param name="options">Options.</param>
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

    /// <summary>
    /// Gets the document for a given rendering job.
    /// </summary>
    /// <returns>The document.</returns>
    /// <param name="job">Job.</param>
    protected virtual IZptDocument GetDocument(IRenderingJob job)
    {
      return job.GetDocument();
    }

    /// <summary>
    /// Gets the output info for a given rendering job.
    /// </summary>
    /// <returns>The output info.</returns>
    /// <param name="job">Job.</param>
    /// <param name="batchOptions">Batch options.</param>
    protected virtual string GetOutputInfo(IRenderingJob job, IBatchRenderingOptions batchOptions)
    {
      return job.GetOutputInfo(batchOptions);
    }

    /// <summary>
    /// Gets the output stream for a given rendering job.
    /// </summary>
    /// <returns>The output stream.</returns>
    /// <param name="job">Job.</param>
    /// <param name="batchOptions">Batch options.</param>
    protected virtual Stream GetOutputStream(IRenderingJob job, IBatchRenderingOptions batchOptions)
    {
      return job.GetOutputStream(batchOptions);
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderer"/> class.
    /// </summary>
    /// <param name="renderingJobFactory">Rendering job factory.</param>
    public BatchRenderer(IRenderingJobFactory renderingJobFactory = null)
    {
      _jobFactory = renderingJobFactory?? new RenderingJobFactory();
    }

    #endregion
  }
}

