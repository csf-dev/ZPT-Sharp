using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;

namespace ZptSharp
{
    /// <summary>
    /// An entry-point object for use by consuming logic.  Renders ZPT documents
    /// from a filesystem file and returns the rendered document as a stream.
    /// </summary>
    public interface IRendersZptFile
    {
        /// <summary>
        /// Renders a specified ZPT document file using the specified model.
        /// </summary>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="filePath">The path of the document file to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="config">An optional rendering configuration object.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task<Stream> RenderAsync(string filePath,
                                 object model,
                                 RenderingConfig config = null,
                                 CancellationToken token = default);
    }
}
