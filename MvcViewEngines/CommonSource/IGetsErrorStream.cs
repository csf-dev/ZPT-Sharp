using System;
using System.IO;
using System.Threading.Tasks;

#if MVC5
namespace ZptSharp.Mvc5
#elif MVCCORE
namespace ZptSharp.MvcCore
#endif
{
    /// <summary>
    /// An object which can get a stream which contains an error page, to show the end user, when rendering fails.
    /// </summary>
    public interface IGetsErrorStream
    {
        /// <summary>
        /// Gets a stream which represents the rendered error document.
        /// </summary>
        /// <returns>The error stream.</returns>
        /// <param name="exception">The exception which caused this error view to be displayed.</param>
        Task<Stream> GetErrorStreamAsync(Exception exception);
    }
}
