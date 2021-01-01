using System.Threading;
using System.Threading.Tasks;
using ZptSharp.BulkRendering;

namespace ZptSharp.Cli
{
    /// <summary>
    /// An object which gets a <see cref="BulkRenderingRequest" /> from a
    /// <see cref="CliArguments" />
    /// </summary>
    public interface IGetsBulkRenderingRequest
    {
        /// <summary>
        /// Gets the bulk rendering request.
        /// </summary>
        /// <param name="args">The command line args.</param>
        /// <param name="cancellationToken">A optional cancellation token.</param>
        /// <returns>A bulk rendering request.</returns>
        Task<BulkRenderingRequest> GetRequestAsync(CliArguments args, CancellationToken cancellationToken = default);
    }
}