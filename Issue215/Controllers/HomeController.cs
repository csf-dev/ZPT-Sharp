using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZPT.Models;

namespace ZPT.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new HomeModel()
            {
                Name = "Mister Piglet",
                DateOfBirth = new DateTime(1972, 5, 20),
                Number = 10,
                Version = new Version(1,1,1,1)
            };

            return View(model);
        }
    }
}