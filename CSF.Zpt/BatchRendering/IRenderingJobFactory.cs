using System;
using System.Collections.Generic;

namespace CSF.Zpt.BatchRendering
{
  /// <summary>
  /// Interface for a factory type which creates instances of <see cref="IRenderingJob"/>.
  /// </summary>
  public interface IRenderingJobFactory
  {
    /// <summary>
    /// Gets the rendering jobs from the batch rendering options.
    /// </summary>
    /// <returns>The rendering jobs.</returns>
    /// <param name="inputOutputInfo">Input output info.</param>
    /// <param name="mode">Mode.</param>
    IEnumerable<IRenderingJob> GetRenderingJobs(IBatchRenderingOptions inputOutputInfo, RenderingMode? mode);
  }
}

