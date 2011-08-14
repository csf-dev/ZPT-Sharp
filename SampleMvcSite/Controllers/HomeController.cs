using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using SampleMvcSite.Content;

namespace Controllers
{
  [HandleError]
  public class HomeController : Controller
  {
    public ActionResult Index ()
    {
      Member member = new Member();
      
      member.Username = "Craig Fowler";
      member.Age = 29;
      
      ViewData["Message"] = "Welcome to ASP.NET MVC on Mono!";
      ViewData["currentUser"] = member;
      
      return View ("macro");
    }
  }
}

