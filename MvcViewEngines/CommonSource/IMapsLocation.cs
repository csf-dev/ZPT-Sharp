namespace ZptSharp.Mvc
{
    /// <summary>
    /// An object which can map a file path (which might be a 'virtual' one)
    /// into an actual file path.
    /// </summary>
    public interface IMapsLocation
    {
        /// <summary>
        /// Maps the location to file path.
        /// </summary>
        /// <param name="location">The location to map.</param>
        /// <returns>The mapped file path.</returns>
        string MapLocation(string location);
    }
}