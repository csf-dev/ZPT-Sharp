using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
namespace CraigFowler.Samples.Mvc.Controllers
{
  public class TestController : Controller
  {
    public ActionResult Index()
    {
      return View("test-page");
    }
  }
}

