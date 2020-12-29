using System.Web.Mvc;

namespace ZptSharp.Examples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new { Message = "Hello world!" });
        }
    }
}
