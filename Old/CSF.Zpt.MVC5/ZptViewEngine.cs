using System;
using System.Web.Mvc;
using CSF.Zpt.Rendering;
using CSF.Zpt.MVC.Rendering;
using System.Text;
using CSF.Configuration;
using CSF.Zpt.Tales;

namespace CSF.Zpt.MVC
{
  public class ZptViewEngine : VirtualPathProviderViewEngine
  {
    #region constants

    private static readonly string[] DefaultViewLocations = new [] {
      "~/Views/{1}/{0}.pt",
      "~/Views/Shared/{0}.pt",
      "~/Views/{1}/{0}.xml",
      "~/Views/Shared/{0}.xml",
    };

    #endregion

    #region fields

    private RenderingMode? _renderingMode;

    #endregion

    #region properties

    /// <summary>
    /// Gets the factory implementation with which to create <see cref="RenderingContext"/> instances.
    /// </summary>
    /// <value>The rendering context factory.</value>
    public IRenderingContextFactory ContextFactory
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether source file annotations should be added to the rendered output.
    /// </summary>
    /// <value><c>true</c> if source file annotations are to be added; otherwise, <c>false</c>.</value>
    public virtual bool AddSourceFileAnnotation
    {
      get;
      set;
    }

    /// <summary>
    /// Gets the context visitors to be used when processing ZPT documents.
    /// </summary>
    /// <value>The context visitors.</value>
    public virtual IContextVisitor[] ContextVisitors
    {
      get;
      set;
    }

    /// <summary>
    /// Gets the encoding (eg: Unicode) for the rendered output.
    /// </summary>
    /// <value>The output encoding.</value>
    public virtual Encoding OutputEncoding
    {
      get;
      set;
    }

    /// <summary>
    /// Gets a value indicating whether the XML declaration should be omitted (where applicable).
    /// </summary>
    /// <value><c>true</c> if the XML declaration is to be omitted; otherwise, <c>false</c>.</value>
    public virtual bool OmitXmlDeclaration
    {
      get;
      set;
    }

    /// <summary>
    /// Gets a string used to indicate a single level of indentation to use when rendering an XML document.
    /// </summary>
    /// <value>The XML indentation characters.</value>
    public virtual string XmlIndentationCharacters
    {
      get;
      set;
    }

    /// <summary>
    /// Gets a value indicating whether XML documents should be rendered with indentated formatting or not.
    /// </summary>
    /// <value><c>true</c> if the rendering process is to output indented XML; otherwise, <c>false</c>.</value>
    public virtual bool OutputIndentedXml
    {
      get;
      set;
    }

    public ITemplateFileFactory TemplateFactory
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

    public Encoding ForceInputEncoding
    {
      get;
      set;
    }

    #endregion

    #region overrides

    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
      return new ZptView(controllerContext.HttpContext.Server.MapPath(viewPath)) {
        RenderingOptions = CreateRenderingOptions(),
        RenderingMode = RenderingMode,
        ForceInputEncoding = ForceInputEncoding,
      };
    }

    protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
    {
      return new ZptView(controllerContext.HttpContext.Server.MapPath(partialPath)) {
        RenderingOptions = CreateRenderingOptions(),
        RenderingMode = RenderingMode,
        ForceInputEncoding = ForceInputEncoding,
      };
    }

    #endregion

    #region methods

    private void InitialiseDefaultViewLocations(string[] locations)
    {
      this.ViewLocationFormats = locations?? DefaultViewLocations;
      this.PartialViewLocationFormats = locations?? DefaultViewLocations;
    }

    private void InitialiseDefaultOptions()
    {
      var defaultOptions = RenderingSettings.Default;
      ContextFactory = new MvcRenderingContextFactory();
      AddSourceFileAnnotation = defaultOptions.AddSourceFileAnnotation;
      ContextVisitors = defaultOptions.ContextVisitors;
      OutputEncoding = defaultOptions.OutputEncoding;
      OmitXmlDeclaration = true;

      RenderingMode = null;
      ForceInputEncoding = null;
    }

    private void InitialiseFromConfig(IZptViewEngineConfiguration config,
                                      IRenderingContextFactoryFactory contextFactoryFactory,
                                      IContextVisitorFactory contextVisitorFactory)
    {
      var defaultOptions = RenderingSettings.Default;

      if(config.ContextFactoryTypeName != null)
      {
        ContextFactory = contextFactoryFactory.Create(config.ContextFactoryTypeName);
      }
      else
      {
        ContextFactory = new MvcRenderingContextFactory();
      }

      if(config.ContextVisitorTypeNames != null)
      {
        ContextVisitors = contextVisitorFactory.CreateMany(config.ContextVisitorTypeNames);
      }
      else
      {
        ContextVisitors = defaultOptions.ContextVisitors;
      }

      if(!String.IsNullOrEmpty(config.ForceInputEncoding))
      {
        ForceInputEncoding = Encoding.GetEncoding(config.ForceInputEncoding);
      }
      else
      {
        ForceInputEncoding = null;
      }

      AddSourceFileAnnotation = config.AddSourceFileAnnotation;
      OutputEncoding = Encoding.GetEncoding(config.OutputEncoding);
      OmitXmlDeclaration = config.OmitXmlDeclaration;
      XmlIndentationCharacters = config.XmlIndentationCharacters;
      OutputIndentedXml = config.OutputIndentedXml;
      RenderingMode = (RenderingMode) Enum.Parse(typeof(RenderingMode), config.RenderingMode);
    }

    private IZptViewEngineConfiguration GetConfiguration()
    {
      var reader = new ConfigurationReader();
      return reader.ReadSection<ZptViewEngineConfigurationSection>();
    }

    private IRenderingSettings CreateRenderingOptions()
    {
      return new RenderingSettings(ContextVisitors,
                                         ContextFactory,
                                         AddSourceFileAnnotation,
                                         OutputEncoding,
                                         OmitXmlDeclaration,
                                   TemplateFactory);
    }

    #endregion

    #region constructor

    public ZptViewEngine(IZptViewEngineConfiguration config = null,
                         string[] viewLocations = null,
                         IRenderingContextFactoryFactory contextFactoryFactory = null,
                         IContextVisitorFactory contextVisitorFactory = null)
    {
      InitialiseDefaultViewLocations(viewLocations);

      config = config?? GetConfiguration();

      if(config != null)
      {
        InitialiseFromConfig(config,
                             contextFactoryFactory?? new RenderingContextFactoryFactory(),
                             contextVisitorFactory?? new ContextVisitorFactory());
      }
      else
      {
        InitialiseDefaultOptions();
      }

      TemplateFactory = new ZptDocumentFactory();
    }

    #endregion
  }
}

