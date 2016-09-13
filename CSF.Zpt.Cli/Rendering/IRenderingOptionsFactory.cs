using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Cli.Rendering
{
  public interface IRenderingOptionsFactory
  {
    IRenderingOptions GetOptions(CommandLineOptions options);
  }
}

