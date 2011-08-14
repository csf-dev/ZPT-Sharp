using System;
using System.Web.Mvc;
using System.Web;
using System.IO;
using CraigFowler.Web.ZPT.Tales;

namespace CraigFowler.Web.ZPT
{
  /// <summary>
  /// <para>Provides an MVC2 <see cref="IViewEngine"/> implementation for serving ZPT pages.</para>
  /// </summary>
  public class ZptViewEngine : VirtualPathProviderViewEngine, IViewEngine
  {
    #region constants
    
    private const string ZPT_VIEW_CACHE_KEY = "zpt_view_cache";
    
    #endregion
    
    #region properties
    
    /// <summary>
    /// <para></para>
    /// </summary>
    public string ViewRoot
    {
      get;
      set;
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// <para>Gets a single view based on its path relative to the root of the view cache.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="ZptDocument"/>
    /// </returns>
    public ZptDocument GetZptDocument(string path)
    {
      ZptDocument output;
      ZptDocumentCollection viewCache = GetViewCache();
      
      output = viewCache.RetrieveItem(ReformatPath(path)) as ZptDocument;
      
      if(output == null)
      {
        throw new ArgumentException(String.Format("No ZPT document was found at the path {0}", path));
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Gets the view cache, optionally creating it if it does not exist.</para>
    /// </summary>
    /// <returns>
    /// A <see cref="ZptDocumentCollection"/>
    /// </returns>
    public ZptDocumentCollection GetViewCache()
    {
      ZptDocumentCollection output;
      
      output = ViewCache;
      
      if(output == null)
      {
        this.RefreshViewCache();
        output = ViewCache;
      }
      
      return output;
    }
    
    /// <summary>
    /// <para>Overloaded.  Refreshes the view cache, creating it from scratch.</para>
    /// </summary>
    public void RefreshViewCache()
    {
      this.RefreshViewCache(HttpContext.Current.Server.MapPath(this.ViewRoot));
    }
    
    /// <summary>
    /// <para>Overloaded.  Refreshes the view cache, creating it from scratch.</para>
    /// </summary>
    /// <param name="basePath">
    /// A <see cref="System.String"/>
    /// </param>
    protected void RefreshViewCache(string basePath)
    {
      DirectoryInfo path = new DirectoryInfo(basePath);
      
      ViewCache = ZptDocumentCollection.CreateFromFilesystem(path);
    }
    
    /// <summary>
    /// <para>Overridden.  Creates a view instance.</para>
    /// </summary>
    /// <param name="controllerContext">
    /// A <see cref="ControllerContext"/>
    /// </param>
    /// <param name="viewPath">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="masterPath">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="IView"/>
    /// </returns>
    protected override IView CreateView (ControllerContext controllerContext, string viewPath, string masterPath)
    {
      if(!String.IsNullOrEmpty(masterPath))
      {
        throw new NotSupportedException("Master views are not supported in the ZPT ViewEngine.");
      }
      
      return new ZptView(viewPath, this);
    }
    
    /// <summary>
    /// <para>Overridden.  Creates a partial view instance.</para>
    /// </summary>
    /// <param name="controllerContext">
    /// A <see cref="ControllerContext"/>
    /// </param>
    /// <param name="partialPath">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="IView"/>
    /// </returns>
    protected override IView CreatePartialView (ControllerContext controllerContext, string partialPath)
    {
      return new ZptView(partialPath, this);
    }
    
    /// <summary>
    /// <para>Reformats a <paramref name="path"/> into one that is more friendly to the <see cref="ViewCache"/>.</para>
    /// </summary>
    /// <param name="path">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.String"/>
    /// </returns>
    protected string ReformatPath(string path)
    {
      string output;
      
      if(path == null)
      {
        throw new ArgumentNullException("path");
      }
      
      if(path.StartsWith(this.ViewRoot))
      {
        output = path.Substring(this.ViewRoot.Length + 1);
      }
      else
      {
        output = path;
      }
      
      if(output.EndsWith(ZptMetadata.ZptTemplateDocumentExtension))
      {
        output = output.Substring(0, output.Length - ZptMetadata.ZptTemplateDocumentExtension.Length);
      }
      
      return String.Format("documents/{0}", output);
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance with a default views location.</para>
    /// </summary>
    public ZptViewEngine ()
    {
      base.ViewLocationFormats = new string[] { "~/Views/{1}/{0}.pt" };
      base.PartialViewLocationFormats = base.ViewLocationFormats;
      
      this.ViewRoot = "~/Views";
    }
    
    #endregion
    
    #region static properties
    
    /// <summary>
    /// <para>Gets and sets the cache of view documents.</para>
    /// </summary>
    protected static ZptDocumentCollection ViewCache
    {
      get {
        return (HttpContext.Current.Application[ZPT_VIEW_CACHE_KEY] as ZptDocumentCollection);
      }
      set {
        HttpContext.Current.Application[ZPT_VIEW_CACHE_KEY] = value;
      }
    }
    
    #endregion
  }
}

