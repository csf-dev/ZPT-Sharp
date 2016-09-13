using System;
using CSF.Zpt.BatchRendering;

namespace CSF.Zpt.Cli.BatchRendering
{
  public interface IBatchRenderingOptionsFactory
  {
    IBatchRenderingOptions GetBatchOptions(CommandLineOptions options);
  }
}

