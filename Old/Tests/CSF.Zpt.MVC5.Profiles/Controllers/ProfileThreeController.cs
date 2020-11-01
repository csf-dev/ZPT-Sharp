using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace CSF.Zpt.MVC5.Profiles.Controllers
{
  public class ProfileThreeController : Controller
  {
    public ActionResult ZopePageTemplates()
    {
      return View();
    }

    public ActionResult Razor()
    {
      return View();
    }

    public class Model
    {
      public Model()
      {
        var viewsPath = System.Web.HttpContext.Current.Server.MapPath("~/Views/");
        var viewsDirectory = new System.IO.DirectoryInfo(viewsPath);
        Views = new Tales.TemplateDirectory(viewsDirectory);
      }

      public Tales.TemplateDirectory Views { get; }
    }
  }
}
