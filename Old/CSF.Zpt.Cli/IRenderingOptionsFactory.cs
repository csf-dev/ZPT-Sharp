using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Cli
{
  public interface IRenderingOptionsFactory
  {
    IRenderingOptions GetOptions(CommandLineOptions options);
  }
}

