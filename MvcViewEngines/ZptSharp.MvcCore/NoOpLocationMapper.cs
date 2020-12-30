namespace ZptSharp.Mvc
{
    /// <summary>
    /// A no-op implementation of <see cref="IMapsLocation" />.
    /// In MVC Core, there is no mapping to be performed between virtual paths and real ones.
    /// </summary>
    public class NoOpLocationMapper : IMapsLocation
    {
        /// <summary>
        /// Maps the location to file path.
        /// </summary>
        /// <param name="location">The location to map.</param>
        /// <returns>The mapped file path.</returns>
        public string MapLocation(string location) => location;
    }
}