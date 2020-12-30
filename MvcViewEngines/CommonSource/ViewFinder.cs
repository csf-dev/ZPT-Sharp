using System;
using System.Collections.Generic;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// Implementation of <see cref="IFindsView" /> which searches-for and
    /// finds view files.
    /// </summary>
    public class ViewFinder : IFindsView
    {
        readonly IMapsLocation mapper;
        readonly ITestForFileExistence fileTester;

        /// <summary>
        /// Attempts to find a view, and returns a result indicating the outcome of the search.
        /// </summary>
        /// <param name="controllerName">The controller name for the desired view.</param>
        /// <param name="viewName">The name of the desired view.</param>
        /// <param name="searchLocationFormats">A collection of locations to search.</param>
        /// <returns>A result indicating success or failure.</returns>
        public FindViewResult FindView(string controllerName,
                                       string viewName,
                                       string[] searchLocationFormats)
        {
            if (controllerName is null)
                throw new System.ArgumentNullException(nameof(controllerName));
            if (viewName is null)
                throw new System.ArgumentNullException(nameof(viewName));
            if (searchLocationFormats is null)
                throw new System.ArgumentNullException(nameof(searchLocationFormats));

            var unsuccessfulLocations = new List<string>();

            foreach(var locationFormat in searchLocationFormats)
            {
                var location = String.Format(locationFormat, viewName, controllerName);
                var path = mapper.MapLocation(location);

                if(fileTester.DoesFileExist(path))
                    return new FindViewResult(path);
                
                unsuccessfulLocations.Add(location);
            }

            return new FindViewResult(unsuccessfulLocations.ToArray());
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ViewFinder" />
        /// </summary>
        /// <param name="mapper">A service to map virtual paths to real ones.</param>
        /// <param name="fileTester">A service to test for the existence of a file.</param>
        public ViewFinder(IMapsLocation mapper, ITestForFileExistence fileTester)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.fileTester = fileTester ?? throw new ArgumentNullException(nameof(fileTester));
        }
    }
}

