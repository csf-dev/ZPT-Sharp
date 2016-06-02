using System;
using System.Web.Mvc;
using System.IO;
using CSF.Zpt.Tales;
using CSF.Zpt.Rendering;
using System.Collections.Generic;
using CSF.Zpt.MVC.Tales;

namespace CSF.Zpt.MVC
{
  public class ZptView : IView
  {
    #region fields

    private string _physicalPath;
    private static IZptDocumentFactory _documentFactory;
    private RenderingMode _renderingMode;

    #endregion

    #region properties

    public RenderingOptions RenderingOptions
    {
      get;
      set;
    }

    public RenderingMode RenderingMode
    {
      get {
        return _renderingMode;
      }
      set {
        if(!value.IsDefinedValue())
        {
          throw new ArgumentException(Resources.ExceptionMessages.InvalidRenderingMode, nameof(value));
        }

        _renderingMode = value;
      }
    }

    public System.Text.Encoding InputEncoding
    {
      get;
      set;
    }

    #endregion

    #region methods

    public void Render(ViewContext viewContext, TextWriter writer)
    {
      var doc = CreateDocument();
      doc.Render(writer,
                 contextConfigurator: ConfigureContext(viewContext),
                 options: RenderingOptions);
    }

    private Action<RenderingContext> ConfigureContext(ViewContext viewContext)
    {
      return c => {
        ConfigureModel(viewContext, c.TalModel);
        ConfigureModel(viewContext, c.MetalModel);
      };
    }

    private void ConfigureModel(ViewContext viewContext, IModel model)
    {
      var typedModel = model as MvcTalesModel;

      if(typedModel != null)
      {
        typedModel.ViewContext = viewContext;
      }
    }

    private IZptDocument CreateDocument()
    {
      IZptDocument output;

      var inputFile = new FileInfo(_physicalPath);

      switch(RenderingMode)
      {
      case RenderingMode.AutoDetect:
        output = _documentFactory.CreateDocument(inputFile, InputEncoding);
        break;

      case RenderingMode.Html:
        output = _documentFactory.CreateHtmlDocument(inputFile, InputEncoding);
        break;

      case RenderingMode.Xml:
        output = _documentFactory.CreateXmlDocument(inputFile, InputEncoding);
        break;

      default:
        throw new InvalidOperationException();
      }

      return output;
    }

    #endregion

    #region constructor

    public ZptView(string physicalPath)
    {
      _physicalPath = physicalPath;
    }

    static ZptView()
    {
      _documentFactory = new ZptDocumentFactory();
    }

    #endregion
  }
}

