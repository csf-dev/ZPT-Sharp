using System;

namespace CSF.Zpt.Cli
{
  public interface ICommandLineOptionsFactory
  {
    CommandLineOptions GetOptions(string[] rawArguments);
  }
}

