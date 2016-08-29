using System;
using CSF.Zpt.BatchRendering;

namespace CSF.Zpt.Cli
{
  public interface IBatchRenderingOptionsCreator
  {
    IBatchRenderingOptions GetBatchOptions(CommandLineOptions options);
  }
}

