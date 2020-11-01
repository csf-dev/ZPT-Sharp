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

    #region methods

    public IRenderingOptions GetOptions(CommandLineOptions options)
    {
      if(options == null)
      {
        throw new ArgumentNullException(nameof(options));
      }

      IRenderingOptions output = new RenderingOptions() {
        AddSourceFileAnnotation = options.EnableSourceAnnotation,
        OmitXmlDeclaration = options.OmitXmlDeclarations,
        ContextVisitorTypes = options.ContextVisitorClassNames,
        RenderingContextFactoryType = options.RenderingContextFactoryClassName,
        OutputEncodingName = options.OutputEncoding
      };

      AddKeywordOptions(options, ref output);

      return output;
    }

    private void AddKeywordOptions(CommandLineOptions options, ref IRenderingOptions renderingOptions)
    {
      if(!String.IsNullOrEmpty(options.KeywordOptions))
      {
        var keywordOptions = options.KeywordOptions
          .Split(KEYWORD_OPTION_SEPARATOR)
          .Select(GetKeywordOption);

        foreach(var option in keywordOptions)
        {
          renderingOptions.KeywordOptions.Add(option.Item1, option.Item2);
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

    #endregion
  }
}

