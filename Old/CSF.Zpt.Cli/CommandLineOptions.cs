using System;
using System.Text;
using System.Collections.Generic;
using CSF.Zpt.Cli.Exceptions;
using CSF.Zpt.Cli.Resources;

namespace CSF.Zpt.Cli
{
  public class CommandLineOptions
  {
    #region fields

    private IList<string> _inputPaths;

    #endregion

    #region properties

    public bool ForceHtmlMode { get; set; }

    public bool ForceXmlMode { get; set; }

    public IList<string> InputPaths
    {
      get {
        return _inputPaths;
      }
      set {
        if(value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        _inputPaths = value;
      }
    }

    public string InputFilenamePattern { get; set; }

    public string OutputPath { get; set; }

    public string OutputFilenameExtension { get; set; }

    public string IgnoredPaths { get; set; }

    public string KeywordOptions { get; set; }

    public string RenderingContextFactoryClassName { get; set; }

    public bool EnableSourceAnnotation { get; set; }

    public string OutputEncoding { get; set; }

    public bool OmitXmlDeclarations { get; set; }

    public string ContextVisitorClassNames { get; set; }

    public bool ShowUsageStatement { get; set; }

    public bool ShowVersionInfo { get; set; }

    public bool VerboseMode { get; set; }

    public bool QuietMode { get; set; }

    #endregion

    #region methods

    public RenderingMode? GetRenderingMode()
    {
      RenderingMode? output;

      if(ForceHtmlMode && ForceXmlMode)
      {
        throw new RenderingModeDeterminationException(ExceptionMessages.HtmlAndXmlModesMutuallyExclusive);
      }

      if(ForceXmlMode)
      {
        output = RenderingMode.Xml;
      }
      else if(ForceHtmlMode)
      {
        output = RenderingMode.Html;
      }
      else
      {
        output = null;
      }

      return output;
    }

    public bool ShowVerboseOutput()
    {
      return this.VerboseMode && !this.QuietMode;
    }

    public bool ShowNormalOutput()
    {
      return !this.QuietMode;
    }

    #endregion

    #region constructor

    public CommandLineOptions()
    {
      InputFilenamePattern = "*.pt";
      OutputEncoding = Encoding.UTF8.WebName;
      _inputPaths = new List<string>();
    }

    #endregion
  }
}

