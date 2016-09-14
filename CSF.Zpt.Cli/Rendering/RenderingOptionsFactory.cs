using System;
using CSF.Zpt.Rendering;
using System.Linq;
using System.Text;

namespace CSF.Zpt.Cli.Rendering
{
  public class RenderingOptionsFactory : IRenderingOptionsFactory
  {
    #region constants

    private const char
    KEYWORD_OPTION_SEPARATOR  = ';',
    KEY_VALUE_SEPARATOR       = '=';

    #endregion

    #region fields

    private readonly IContextVisitorFactory _contextVisitorFactory;
    private readonly IRenderingContextFactoryFactory _contextFactoryFactory;

    #endregion

    #region methods

    public IRenderingOptions GetOptions(CommandLineOptions options)
    {
      try
      {
        var contextVisitors = _contextVisitorFactory.CreateMany(options.ContextVisitorClassNames);
        var contextFactory = _contextFactoryFactory.Create(options.RenderingContextFactoryClassName);

        AddKeywordOptions(options, contextFactory);

        return new RenderingOptions(contextVisitors,
                                    contextFactory,
                                    addSourceFileAnnotation: options.EnableSourceAnnotation,
                                    outputEncoding: Encoding.GetEncoding(options.OutputEncoding),
                                    omitXmlDeclaration: options.OmitXmlDeclarations);
      }
      catch(Exception ex)
      {
        throw new OptionsParsingException(Resources.Messages.RenderingOptionsCreationExceptionMessage, ex);
      }
    }

    private void AddKeywordOptions(CommandLineOptions options, IRenderingContextFactory contextFactory)
    {
      if(!String.IsNullOrEmpty(options.KeywordOptions))
      {
        var keywordOptions = options.KeywordOptions
          .Split(KEYWORD_OPTION_SEPARATOR)
          .Select(x => {
          var keyAndValue = x.Split(KEY_VALUE_SEPARATOR);
          return (keyAndValue.Length == 2)? new { Key = keyAndValue[0], Value = keyAndValue[1] } : null;
        })
          .Where(x => x != null);

        foreach(var option in keywordOptions)
        {
          contextFactory.AddKeywordOption(option.Key, option.Value);
        }
      }
    }

    #endregion

    #region constructor

    public RenderingOptionsFactory(IContextVisitorFactory contextVisitorFactory = null,
                                   IRenderingContextFactoryFactory contextFactoryFactory = null)
    {
      _contextVisitorFactory = contextVisitorFactory?? new ContextVisitorFactory();
      _contextFactoryFactory = contextFactoryFactory?? new RenderingContextFactoryFactory();
    }

    #endregion
  }
}

