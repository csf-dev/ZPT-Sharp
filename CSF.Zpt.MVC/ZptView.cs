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
    #region constants

    private const string
      VIEWS_TALES_PATH    = "views",
      VIEWS_VIRTUAL_PATH  = "~/Views/";

    #endregion

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
        ConfigureViewAndTempData(viewContext, c.TalModel);
        ConfigureViewAndTempData(viewContext, c.MetalModel);

        var viewsDirectoryPath = viewContext.HttpContext.Server.MapPath(VIEWS_VIRTUAL_PATH);
        var viewsDirectory = new TemplateDirectory(new DirectoryInfo(viewsDirectoryPath));

        c.TalModel.AddGlobal(VIEWS_TALES_PATH, viewsDirectory);
        c.MetalModel.AddGlobal(VIEWS_TALES_PATH, viewsDirectory);
      };
    }

    private void ConfigureViewAndTempData(ViewContext viewContext, IModel model)
    {
      var typedModel = model as MvcTalesModel;

      if(typedModel != null)
      {
        typedModel.SetViewData(viewContext.ViewData);
        typedModel.SetTempData(viewContext.TempData);
      }
      else
      {
        foreach(var key in viewContext.ViewData.Keys)
        {
          model.AddGlobal(key, viewContext.ViewData[key]);
        }
        foreach(var key in viewContext.TempData.Keys)
        {
          model.AddGlobal(key, viewContext.TempData[key]);
        }
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

