using System;
using System.Web.Mvc;
using ZptSample.Models;

namespace ZptSample.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      var model = new HomeModel() {
        Name = "Sam Smith",
        DateOfBirth = new DateTime(1990, 5, 20),
      };

      return View(model);
    }

    public ActionResult ErrorPage()
    {
      return View();
    }
  }
}
