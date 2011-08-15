//  
//  ZptView.cs
//  
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
// 
//  Copyright (c) 2011 Craig Fowler
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Web.Mvc;
using System.IO;

namespace CraigFowler.Web.ZPT.Mvc
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
      IZptDocument document = this.Engine.GetZptDocument(this.RelativeViewPath);
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

