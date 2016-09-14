using System;
using CSF.Cli;

namespace CSF.Zpt.Cli
{
  public class ParameterParserFactory : IParameterParserFactory
  {
    #region fields

    private IParameterParser<CommandLineOptions> _parser;

    #endregion

    #region methods

    /// <summary>
    /// Creates and returns a parameter parser for the application.
    /// </summary>
    /// <returns>The parameter parser.</returns>
    public IParameterParser<CommandLineOptions> GetParser()
    {
      if(_parser == null)
      {
        _parser = CreateParser();
      }

      return _parser;
    }

    private IParameterParser<CommandLineOptions> CreateParser()
    {
      return new ParameterParserBuilder<CommandLineOptions>()
        .AddFlag(   x => x.ForceHtmlMode,                     longName: "html",                       shortName: "t")
        .AddFlag(   x => x.ForceXmlMode,                      longName: "xml",                        shortName: "x")
        .AddFlag(   x => x.EnableSourceAnnotation,            longName: "annotate")
        .AddFlag(   x => x.OmitXmlDeclarations,               longName: "no-xml-declaration")
        .AddFlag(   x => x.ShowUsageStatement,                longName: "help",                       shortName: "h")
        .AddFlag(   x => x.ShowVersionInfo,                   longName: "version",                    shortName: "v")
        .AddValue(  x => x.InputFilenamePattern,              longName: "input-filename-pattern",     shortName: "p", optional: false)
        .AddValue(  x => x.OutputPath,                        longName: "output",                     shortName: "o", optional: false)
        .AddValue(  x => x.OutputFilenameExtension,           longName: "output-filename-extension",  shortName: "e", optional: false)
        .AddValue(  x => x.IgnoredPaths,                      longName: "ignore",                     shortName: "i", optional: false)
        .AddValue(  x => x.KeywordOptions,                    longName: "keyword-options",                            optional: false)
        .AddValue(  x => x.RenderingContextFactoryClassName,  longName: "rendering-context-factory",                  optional: false)
        .AddValue(  x => x.OutputEncoding,                    longName: "output-encoding",            shortName: "n", optional: false)
        .AddValue(  x => x.ContextVisitorClassNames,          longName: "context-visitors",                           optional: false)
        .RemainingArguments(x => x.InputPaths)
        .Build();
    }

    #endregion
  }
}

