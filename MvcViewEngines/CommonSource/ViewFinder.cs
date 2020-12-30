using System;
using System.Collections.Generic;
using System.Linq;
#if MVC5
using System.Web.Mvc;
using System.Web;
#elif MVCCORE
using Microsoft.AspNetCore.Mvc.ViewEngines;
#endif
using System.IO;

namespace ZptSharp.Mvc
{
    internal static class ViewFinder
    {
        internal static FindViewResult FindView(string controllerName,
                                                string viewName,
                                                string[] searchLocationFormats)
        {
            if (controllerName is null)
                throw new System.ArgumentNullException(nameof(controllerName));
            if (viewName is null)
                throw new System.ArgumentNullException(nameof(viewName));
            if (searchLocationFormats is null)
                throw new System.ArgumentNullException(nameof(searchLocationFormats));

            var attemptedLocations = new List<string>();

            foreach(var locationFormat in searchLocationFormats)
            {
                var location = String.Format(locationFormat, viewName, controllerName);
                attemptedLocations.Add(location);
                var path = GetViewPath(location);

                if(File.Exists(path))
                    return new FindViewResult { Success = true, Path = path };
            }

            return new FindViewResult { Success = false, AttemptedLocations = attemptedLocations.ToArray() };
        }

        static string GetViewPath(string location)
        {
#if MVCCORE
            return location;
#elif MVC5
            return HttpContext.Current.Server.MapPath(location);
#endif
        }

        internal class FindViewResult
        {
            internal bool Success { get; set; }
            internal string Path { get; set; }
            internal string[] AttemptedLocations { get; set; }
        }
    }
}

