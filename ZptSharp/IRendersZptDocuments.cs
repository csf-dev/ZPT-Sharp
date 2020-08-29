using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Rendering;

namespace ZptSharp
{
    /// <summary>
    /// An entry-point object for use by consuming logic.  Renders ZPT documents
    /// from a stream and returns the rendered document as a stream.
    /// </summary>
    public interface IRendersZptDocuments
    {
        /// <summary>
        /// Renders a specified ZPT document from a stream using the specified model.
        /// </summary>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="stream">The stream containing the document to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        /// <param name="contextBuilder">The context builder action.</param>
        Task<Stream> RenderAsync(Stream stream, object model, CancellationToken token, Action<IConfiguresRootContext> contextBuilder = null);
    }
}
