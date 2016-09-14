using System;
using CSF.Cli;

namespace CSF.Zpt.Cli
{
  public interface IParameterParserFactory
  {
    IParameterParser<CommandLineOptions> GetParser();
  }
}

