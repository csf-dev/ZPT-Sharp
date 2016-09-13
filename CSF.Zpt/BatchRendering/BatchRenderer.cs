using System;
using System.IO;
using CSF.IO;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;
using System.Collections.Generic;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Default implementation of <see cref="IBatchRenderer"/>.
  /// </summary>
  public class BatchRenderer : IBatchRenderer
  {
    #region fields

    private RenderingJobFactory _renderingJobFactory;

    #endregion

    #region public API

    /// <summary>
    /// Parse and render the documents found using the given batch rendering options.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    /// <param name="batchOptions">Batch rendering options, indicating the source and destination files.</param>
    /// <param name="mode">An optional override for the rendering mode.</param>
    /// <returns>
    /// An object instance indicating the outcome of the rendering.
    /// </returns>
    public IBatchRenderingResponse Render(IRenderingOptions options,
                                          IBatchRenderingOptions batchOptions,
                                          RenderingMode? mode)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }
      if(batchOptions == null)
      {
        throw new ArgumentNullException(nameof(batchOptions));
      }

      IBatchRenderingResponse output;

      try
      {
        batchOptions.Validate();

        var jobs = _renderingJobFactory.GetRenderingJobs(batchOptions, mode);
        output = RenderJobs(jobs, options, batchOptions);
      }
      catch(BatchRenderingException ex)
      {
        if(batchOptions.ErrorHandlingStrategy == BatchErrorHandlingStrategy.RaiseExceptionForAnyError
           || batchOptions.ErrorHandlingStrategy == BatchErrorHandlingStrategy.RaiseExceptionForFatalContinueOnDocumentError
           || batchOptions.ErrorHandlingStrategy == BatchErrorHandlingStrategy.RaiseExceptionForFatalStopOnFirstDocumentError)
        {
          throw;
        }
        else if(ex.FatalError.HasValue)
        {
          output = new BatchRenderingResponse(ex.FatalError.Value);
        }
        else
        {
          throw;
        }
      }

      return output;
    }

    #endregion

    #region methods

    private IBatchRenderingResponse RenderJobs(IEnumerable<RenderingJob> jobs,
                                               IRenderingOptions options,
                                               IBatchRenderingOptions batchOptions)
    {
      List<IBatchRenderingDocumentResponse> docs = new List<IBatchRenderingDocumentResponse>();

      foreach(var job in jobs)
      {
        var contextConfigurator = GetContextConfigurator(job);
        var docResponse = Render(job, options, batchOptions, contextConfigurator);

        docs.Add(docResponse);

        if(!docResponse.Success &&
           (batchOptions.ErrorHandlingStrategy == BatchErrorHandlingStrategy.NoExpectedExceptionsStopOnFirstDocumentError
            || batchOptions.ErrorHandlingStrategy == BatchErrorHandlingStrategy.RaiseExceptionForFatalStopOnFirstDocumentError))
        {
          break;
        }
      }

      return new BatchRenderingResponse(docs);
    }

    private IBatchRenderingDocumentResponse Render(RenderingJob job,
                                                   IRenderingOptions options,
                                                   IBatchRenderingOptions batchOptions,
                                                   Action<RenderingContext> contextConfigurator)
    {
      IBatchRenderingDocumentResponse output;
      string inputName = job.Document.GetSourceInfo().FullName;

      try
      {
        using(var outputStream = job.GetOutputStream(batchOptions))
        using(var writer = new StreamWriter(outputStream, options.OutputEncoding))
        {
          job.Document.Render(writer,
                              options: options,
                              contextConfigurator: contextConfigurator);
        }

        // TODO: Add output location information
        output = new BatchRenderingDocumentResponse(inputName, "output location");
      }
      catch(RenderingException)
      {
        if(batchOptions.ErrorHandlingStrategy == BatchErrorHandlingStrategy.RaiseExceptionForAnyError)
        {
          throw;
        }
        else
        {
          output = new BatchRenderingDocumentResponse(inputName,
                                                      errorType: BatchRenderingDocumentErrorType.Default);
        }
      }

      return output;
    }

    private Action<RenderingContext> GetContextConfigurator(RenderingJob job)
    {
      return ctx => {
        if(job.InputRootDirectory != null)
        {
          var docRoot = new TemplateDirectory(job.InputRootDirectory);
          ctx.MetalModel.AddGlobal("documents", docRoot);
        }
      };
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.BatchRendering.BatchRenderer"/> class.
    /// </summary>
    public BatchRenderer()
    {
      _renderingJobFactory = new RenderingJobFactory();
    }

    #endregion
  }
}

