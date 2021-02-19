#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// Represents the result from <see cref="IFindsView.FindView(string, string, string[])" />.
    /// </summary>
    public class FindViewResult
    {
        /// <summary>
        /// Gets a value indicating whether or not a view was found.
        /// </summary>
        /// <value>Whether or not the result was a success.</value>
        public bool Success { get; }

        /// <summary>
        /// Gets the file path to the view, assuming that <see cref="Success" /> is
        /// <see langword="true" />.
        /// </summary>
        /// <value>The path to the view file.</value>
        public string Path { get; }

        /// <summary>
        /// Gets a collection of the locations which were unsuccesfully searched, when
        /// <see cref="Success" /> is <see langword="false" />.
        /// </summary>
        /// <value>The searched locations.</value>
        public string[] AttemptedLocations { get; }

        /// <summary>
        /// Initializes an instance of <see cref="FindViewResult" /> when a view was successfully found.
        /// </summary>
        /// <param name="path">The path to the view file.</param>
        public FindViewResult(string path)
        {
            Success = true;
            Path = path ?? throw new System.ArgumentNullException(nameof(path));
        }

        /// <summary>
        /// Initializes an instance of <see cref="FindViewResult" /> when a view was not found.
        /// </summary>
        /// <param name="attemptedLocations">The (unsuccesfully) searched locations.</param>
        public FindViewResult(string[] attemptedLocations)
        {
            Success = false;
            AttemptedLocations = attemptedLocations ?? throw new System.ArgumentNullException(nameof(attemptedLocations));
        }
    }
}