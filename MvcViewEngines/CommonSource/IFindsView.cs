#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// An object which finds a view file, searching a number of provided locations.
    /// </summary>
    public interface IFindsView
    {
        /// <summary>
        /// Attempts to find a view, and returns a result indicating the outcome of the search.
        /// </summary>
        /// <param name="controllerName">The controller name for the desired view.</param>
        /// <param name="viewName">The name of the desired view.</param>
        /// <param name="searchLocationFormats">A collection of locations to search.</param>
        /// <returns>A result indicating success or failure.</returns>
        FindViewResult FindView(string controllerName,
                                string viewName,
                                string[] searchLocationFormats);
    }
}