using System;
using System.Collections.Generic;
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
    #region public API

    /// <summary>
    /// Parse and render the documents found using the given batch rendering options.
    /// </summary>
    /// <param name="options">Rendering options.</param>
    /// <param name="batchOptions">Batch rendering options, indicating the source and destination files.</param>
    /// <param name="mode">An optional override for the rendering mode.</param>
    public virtual BatchRenderingResponse Render(IRenderingOptions options,
                       IBatchRenderingOptions batchOptions,
                       RenderingMode? mode)
    {
        var jobs = GetRenderingJobs(batchOptions, mode);

      foreach(var job in jobs)
      {
          var contextConfigurator = GetContextConfigurator(job);

        Render(job, options, batchOptions, contextConfigurator);
      }

      return new BatchRenderingResponse();
    }

        #endregion

        #region protected methods

        protected virtual void Render(RenderingJob job,
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

      protected virtual RenderingJobFactory GetRenderingJobFactory()
      {
            return new RenderingJobFactory();
        }

      protected virtual IEnumerable<RenderingJob> GetRenderingJobs(IBatchRenderingOptions options, RenderingMode? mode)
      {
          var factory = GetRenderingJobFactory();
          return factory.GetRenderingJobs(options, mode);
      }

      protected virtual Action<RenderingContext> GetContextConfigurator(RenderingJob job)
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
  }
}

