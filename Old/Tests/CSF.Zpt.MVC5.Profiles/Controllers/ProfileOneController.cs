using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace CSF.Zpt.MVC5.Profiles.Controllers
{
  public class ProfileOneController : Controller
  {
    public ActionResult ZopePageTemplates()
    {
      return View();
    }

    public ActionResult Razor()
    {
      return View();
    }
  }
}
