using System;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Cli
{
  public interface IRenderingOptionsFactory
  {
    IRenderingSettings GetOptions(CommandLineOptions options);
  }
}

