using System;
using System.IO;
using CSF.IO;
using CSF.Zpt.Rendering;
using CSF.Zpt.Tales;

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
    public IBatchRenderingResponse Render(IRenderingOptions options,
                                          IBatchRenderingOptions batchOptions,
                                          RenderingMode? mode)
    {
      var jobs = _renderingJobFactory.GetRenderingJobs(batchOptions, mode);

      foreach(var job in jobs)
      {
        Action<RenderingContext> contextConfigurator = ctx => {
          if(job.InputRootDirectory != null)
          {
            var docRoot = new TemplateDirectory(job.InputRootDirectory);
            ctx.MetalModel.AddGlobal("documents", docRoot);
          }
        };

        Render(job, options, batchOptions, contextConfigurator);
      }

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    private void Render(RenderingJob job,
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

