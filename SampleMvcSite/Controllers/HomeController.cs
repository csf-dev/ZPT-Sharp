//  
//  HomeController.cs
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

using System.Web.Mvc;
using CraigFowler.Samples.Mvc.Models;

namespace CraigFowler.Samples.Mvc.Controllers
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

