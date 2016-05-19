using System;
using CSF.Cli;
using System.Linq;
using System.Text;
using CSF.Zpt.Rendering;

namespace CSF.Zpt.Cli
{
  /// <summary>
  /// The application entry point.
  /// </summary>
  public class EntryPoint
  {
    #region fields

    private IParameterParser<CommandLineOptions> _parameterParser;
    private CommandLineOptions _commandLineOptions;
    private IContextVisitorFactory _contextVisitorFactory;
    private IRenderingContextFactoryFactory _contextFactoryFactory;

    #endregion

    #region methods

    /// <summary>
    /// Perform the work of the application.
    /// </summary>
    public void Begin()
    {
      var options = this.CreateRenderingOptions(_commandLineOptions);

      // TODO: Write this implementation
      throw new NotImplementedException();
    }

    private RenderingOptions CreateRenderingOptions(CommandLineOptions options)
    {
      var contextVisitors = _contextVisitorFactory.CreateMany(options.ContextVisitorClassNames);
      var contextFactory = _contextFactoryFactory.Create(options.RenderingContextFactoryClassName);

      return new DefaultRenderingOptions(contextVisitors,
                                         contextFactory,
                                         addSourceFileAnnotation: options.EnableSourceAnnotation,
                                         outputEncoding: Encoding.GetEncoding(options.OutputEncoding),
                                         omitXmlDeclaration: options.OmitXmlDeclarations,
                                         xmlIndentCharacters: options.XmlIndentationCharacters,
                                         outputIndentedXml: !options.DoNotOutputIndentedXml);
    }

    /// <summary>
    /// Creates and returns a parameter parser for the application.
    /// </summary>
    /// <returns>The parameter parser.</returns>
    private IParameterParser<CommandLineOptions> CreateParameterParser()
    {
      return new ParameterParserBuilder<CommandLineOptions>()
        .AddFlag(   x => x.ForceHtmlMode,                     longName: "html",                       shortName: "h")
        .AddFlag(   x => x.ForceXmlMode,                      longName: "xml",                        shortName: "x")
        .AddFlag(   x => x.EnableSourceAnnotation,            longName: "annotate")
        .AddFlag(   x => x.OmitXmlDeclarations,               longName: "no-xml-declaration")
        .AddFlag(   x => x.DoNotOutputIndentedXml,            longName: "no-indent-xml")
        .AddFlag(   x => x.ShowUsageStatement,                longName: "help",                       shortName: "?")
        .AddFlag(   x => x.DoNotAddDocumentsMetalRootObject,  longName: "no-documents-root")
        .AddValue(  x => x.TemplateFilenamePattern,           longName: "template-filename-pattern",  shortName: "t", optional: false)
        .AddValue(  x => x.OutputPath,                        longName: "out",                        shortName: "o", optional: false)
        .AddValue(  x => x.OutputFilenameExtension,           longName: "output-filename-extension",  shortName: "e", optional: false)
        .AddValue(  x => x.IgnoredPaths,                      longName: "ignore",                     shortName: "i", optional: false)
        .AddValue(  x => x.KeywordOptions,                    longName: "keyword-options",                            optional: false)
        .AddValue(  x => x.RenderingContextFactoryClassName,  longName: "rendering-context-factory",                  optional: false)
        .AddValue(  x => x.OutputEncoding,                    longName: "out-encoding",               shortName: "n", optional: false)
        .AddValue(  x => x.XmlIndentationCharacters,          longName: "xml-indent-chars",                           optional: false)
        .AddValue(  x => x.ContextVisitorClassNames,          longName: "context-visitors",                           optional: false)
        .RemainingArguments(x => x.InputPaths)
        .Build();
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Zpt.Cli.EntryPoint"/> class.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <param name="parameterParser">A parameter parser.</param>
    /// <param name="contextVisitorFactory">A context visitor factory.</param>
    /// <param name="contextFactoryFactory">A rendering context factory factory.</param>
    public EntryPoint(string[] args,
                      IParameterParser<CommandLineOptions> parameterParser = null,
                      IContextVisitorFactory contextVisitorFactory = null,
                      IRenderingContextFactoryFactory contextFactoryFactory = null)
    {
      if(args == null)
      {
        throw new ArgumentNullException(nameof(args));
      }

      _parameterParser = parameterParser?? CreateParameterParser();
      _commandLineOptions = _parameterParser.Parse(args);
      _contextVisitorFactory = contextVisitorFactory?? new ContextVisitorFactory();
      _contextFactoryFactory = contextFactoryFactory?? new RenderingContextFactoryFactory();
    }

    #endregion

    #region static methods

    /// <summary>
    /// The entry point of the program, where the program control starts and ends.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
      var app = new EntryPoint(args);

      app.Begin();
    }

    #endregion
  }
}

