using System;
using System.Web.Mvc;
using System.IO;

namespace CraigFowler.Web.ZPT
{
  /// <summary>
  /// <para>Represents an <see cref="IView"/> that represents a ZPT page.</para>
  /// </summary>
  public class ZptView : IView
  {
    #region properties
    
    /// <summary>
    /// <para>Read-only.  Gets the relative path to this view file from the root of the views location.</para>
    /// </summary>
    public string RelativeViewPath
    {
      get;
      private set;
    }
    
    /// <summary>
    /// <para>Read-only.  Gets the ZPT view engine that handles this view.</para>
    /// </summary>
    protected ZptViewEngine Engine  {
      get;
      private set;
    }
    
    #endregion
    
    #region IView implementation
    
    /// <summary>
    /// <para>Renders the current view to the <paramref name="writer"/>.</para>
    /// </summary>
    /// <param name="viewContext">
    /// A <see cref="ViewContext"/>
    /// </param>
    /// <param name="writer">
    /// A <see cref="TextWriter"/>
    /// </param>
    public void Render (ViewContext viewContext, TextWriter writer)
    {
      ZptDocument document = this.Engine.GetZptDocument(this.RelativeViewPath);
      ITemplateDocument template = document.GetTemplateDocument();
      
      template.TalesContext.AddDefinition("documents", this.Engine.GetViewCache());
      foreach(string key in viewContext.ViewData.Keys)
      {
        template.TalesContext.AddDefinition(key, viewContext.ViewData[key]);
      }
      
      writer.Write(template.Render());
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// <para>Initialises this instance for a local relative view path.</para>
    /// </summary>
    /// <param name="viewPath">
    /// A <see cref="System.String"/>
    /// </param>
    /// <param name="engine">
    /// A <see cref="ZptViewEngine"/> pointing to the engine that created this view.
    /// </param>
    public ZptView (string viewPath, ZptViewEngine engine)
    {
      this.RelativeViewPath = viewPath;
      this.Engine = engine;
    }
    
    #endregion
  }
}

