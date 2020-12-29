using System.Web.Mvc;

namespace ZptSharp.Examples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.sample = 123;
            return View(new { Message = "Hello world!" });
        }
    }
}
