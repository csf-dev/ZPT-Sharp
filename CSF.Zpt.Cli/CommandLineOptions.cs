using System;
using System.Text;
using System.Collections.Generic;

namespace CSF.Zpt.Cli
{
  public class CommandLineOptions
  {
    #region properties

    public bool ForceHtmlMode
    {
      get;
      set;
    }

    public bool ForceXmlMode
    {
      get;
      set;
    }

    public IList<string> InputPaths
    {
      get;
      set;
    }

    public string TemplateFilenamePattern
    {
      get;
      set;
    }

    public string OutputPath
    {
      get;
      set;
    }

    public string OutputFilenameExtension
    {
      get;
      set;
    }

    public string IgnoredPaths
    {
      get;
      set;
    }

    public string KeywordOptions
    {
      get;
      set;
    }

    public string RenderingContextFactoryClassName
    {
      get;
      set;
    }

    public bool EnableSourceAnnotation
    {
      get;
      set;
    }

    public string OutputEncoding
    {
      get;
      set;
    }

    public bool OmitXmlDeclarations
    {
      get;
      set;
    }

    public bool DoNotOutputIndentedXml
    {
      get;
      set;
    }

    public string XmlIndentationCharacters
    {
      get;
      set;
    }

    public string ContextVisitorClassNames
    {
      get;
      set;
    }

    public bool ShowUsageStatement
    {
      get;
      set;
    }

    public bool DoNotAddDocumentsMetalRootObject
    {
      get;
      set;
    }

    #endregion

    #region constructor

    public CommandLineOptions()
    {
      TemplateFilenamePattern = "*.pt";
      OutputEncoding = Encoding.UTF8.WebName;
      XmlIndentationCharacters = "  ";
    }

    #endregion
  }
}

