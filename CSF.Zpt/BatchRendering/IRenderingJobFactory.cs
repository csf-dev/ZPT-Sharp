using System;
using System.Collections.Generic;

namespace CSF.Zpt.BatchRendering
{
  public interface IRenderingJobFactory
  {
    IEnumerable<IRenderingJob> GetRenderingJobs(IBatchRenderingOptions inputOutputInfo, RenderingMode? mode);
  }
}

