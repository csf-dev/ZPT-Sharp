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
    private RenderingMode? _renderingMode;

    #endregion

    #region properties

    public IRenderingSettings RenderingOptions
    {
      get;
      set;
    }

    public RenderingMode? RenderingMode
    {
      get {
        return _renderingMode;
      }
      set {
        if(value.HasValue && !value.Value.IsDefinedValue())
        {
          throw new ArgumentException(Resources.ExceptionMessages.InvalidRenderingMode, nameof(value));
        }

        _renderingMode = value;
      }
    }

    public System.Text.Encoding ForceInputEncoding
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

    private Action<IModelValueContainer> ConfigureContext(ViewContext viewContext)
    {
      return c => {
        ConfigureModel(viewContext, c.TalModel);
        ConfigureModel(viewContext, c.MetalModel);
      };
    }

    private void ConfigureModel(ViewContext viewContext, IModelValueStore model)
    {
      var typedModel = model as MvcTalesModel;

      if(typedModel != null)
      {
        typedModel.ViewContext = viewContext;
      }
    }

    private IZptDocument CreateDocument()
    {
      var inputFile = new FileInfo(_physicalPath);
      return _documentFactory.CreateDocument(inputFile, ForceInputEncoding, RenderingMode);
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

