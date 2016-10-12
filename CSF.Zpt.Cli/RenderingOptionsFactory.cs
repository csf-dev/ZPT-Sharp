using System;
using CSF.Zpt.Rendering;
using System.Linq;
using System.Text;
using CSF.Zpt.Cli.Resources;
using CSF.Zpt.Cli.Exceptions;

namespace CSF.Zpt.Cli
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

    public IRenderingSettings GetOptions(CommandLineOptions options)
    {
      var contextVisitors = _contextVisitorFactory.CreateMany(options.ContextVisitorClassNames);
      var contextFactory = _contextFactoryFactory.Create(options.RenderingContextFactoryClassName);
      var encoding = GetEncoding(options.OutputEncoding);

      AddKeywordOptions(options, contextFactory);

      return new RenderingSettings(contextVisitors,
                                  contextFactory,
                                  addSourceFileAnnotation: options.EnableSourceAnnotation,
                                  outputEncoding: encoding,
                                  omitXmlDeclaration: options.OmitXmlDeclarations);
    }

    private void AddKeywordOptions(CommandLineOptions options, IRenderingContextFactory contextFactory)
    {
      if(!String.IsNullOrEmpty(options.KeywordOptions))
      {
        var keywordOptions = options.KeywordOptions
          .Split(KEYWORD_OPTION_SEPARATOR)
          .Select(GetKeywordOption);

        foreach(var option in keywordOptions)
        {
          contextFactory.AddKeywordOption(option.Item1, option.Item2);
        }
      }
    }

    private Tuple<string,string> GetKeywordOption(string input)
    {
      var keyAndValue = input.Split(new [] { KEY_VALUE_SEPARATOR }, 2);

      if(keyAndValue.Length == 2)
      {
        return new Tuple<string,string>(keyAndValue[0], keyAndValue[1]);
      }
      else
      {
        throw new InvalidKeywordOptionsException() {
          InvalidOption = input
        };
      }
    }

    private Encoding GetEncoding(string name)
    {
      try
      {
        return Encoding.GetEncoding(name);
      }
      catch(Exception ex)
      {
        throw new InvalidOutputEncodingException(ExceptionMessages.InvalidEncoding, ex);
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

