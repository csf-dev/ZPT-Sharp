using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp
{
    /// <summary>
    /// An entry-point object for use by consuming logic.  Renders ZPT documents
    /// from a stream and returns the rendered document as a stream.
    /// </summary>
    public interface IRendersZptDocument
    {
        /// <summary>
        /// Renders a specified ZPT document from a stream using the specified model.
        /// </summary>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="stream">The stream containing the document to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="config">An optional rendering configuration object.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        /// <param name="sourceInfo">The source info for the <paramref name="stream"/>.</param>
        Task<Stream> RenderAsync(Stream stream,
                                 object model,
                                 RenderingConfig config = null,
                                 CancellationToken token = default,
                                 IDocumentSourceInfo sourceInfo = null);
    }
}
