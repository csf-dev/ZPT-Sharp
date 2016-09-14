using System;

namespace CSF.Zpt.Cli
{
  public class CommandLineOptionsFactory : ICommandLineOptionsFactory
  {
    #region fields

    private readonly IParameterParserFactory _parserFactory;

    #endregion

    #region methods

    public CommandLineOptions GetOptions(string[] rawArguments)
    {
      var parser = _parserFactory.GetParser();

      try
      {
        return parser.Parse(rawArguments);
      }
      catch(Exception ex)
      {
        throw new OptionsParsingException(Resources.Messages.OptionsParsingExceptionMessage, ex);
      }
    }

    #endregion

    #region constructor

    public CommandLineOptionsFactory(IParameterParserFactory parserFactory = null)
    {
      _parserFactory = parserFactory?? new ParameterParserFactory();
    }

    #endregion
  }
}

