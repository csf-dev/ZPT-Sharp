//  
//  Global.asax.cs
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


using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CraigFowler.Web.ZPT.Mvc.Samples
{
  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterRoutes (RouteCollection routes)
    {
      routes.IgnoreRoute ("{resource}.axd/{*pathInfo}");
      
      routes.MapRoute ("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = "" });
      
    }

    protected void Application_Start ()
    {
      RegisterRoutes (RouteTable.Routes);
      
      ZptMetadata.RegisterDocumentClasses(System.Reflection.Assembly.GetExecutingAssembly());
      
      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(new ZptViewEngine());
    }
  }
}

