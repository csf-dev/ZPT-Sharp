using System.Web;

namespace ZptSharp.Mvc
{
    /// <summary>
    /// Implementation of <see cref="IMapsLocation" /> which uses <see cref="HttpServerUtility.MapPath(string)" />
    /// to map between virtual locations and actual paths.
    /// </summary>
    public class ServerLocationMapper : IMapsLocation
    {
        readonly HttpContextBase context;

        /// <summary>
        /// Maps the location to file path.
        /// </summary>
        /// <param name="location">The location to map.</param>
        /// <returns>The mapped file path.</returns>
        public string MapLocation(string location)
            => context.Server.MapPath(location);

        /// <summary>
        /// Initializes a new instance of <see cref="ServerLocationMapper" />.
        /// </summary>
        /// <param name="context">An HTTP Context exposing a server utility.</param>
        public ServerLocationMapper(HttpContextBase context)
        {
            this.context = context ?? throw new System.ArgumentNullException(nameof(context));
        }
    }
}