using System;
using CSF.Zpt.BatchRendering;

namespace CSF.Zpt.Cli
{
  public interface IBatchRenderingOptionsFactory
  {
    IBatchRenderingOptions GetBatchOptions(CommandLineOptions options);
  }
}

