using System.IO;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Represents a request to render a single ZPT document.
    /// </summary>
    public class RenderZptDocumentRequest
    {
        /// <summary>
        /// Gets the model - arbitrary data passed by the caller
        /// representing the object which is being rendered to the
        /// ZPT document.
        /// </summary>
        /// <value>The model.</value>
        public object Model { get; }

        /// <summary>
        /// Gets a stream containing the document to use for rendering.
        /// </summary>
        /// <value>The document stream.</value>
        public Stream DocumentStream { get; }

        /// <summary>
        /// Gets information about the source of the <see cref="DocumentStream"/>, for example its original file path.
        /// </summary>
        /// <value>The source info.</value>
        public IDocumentSourceInfo SourceInfo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderZptDocumentRequest"/> class.
        /// </summary>
        /// <param name="document">The document stream.</param>
        /// <param name="model">The model.</param>
        /// <param name="sourceInfo">Information which identifies the source of the <paramref name="document"/>.</param>
        public RenderZptDocumentRequest(Stream document,
                                        object model,
                                        IDocumentSourceInfo sourceInfo = null)
        {
            DocumentStream = document ?? throw new System.ArgumentNullException(nameof(document));
            SourceInfo = sourceInfo ?? new UnknownSourceInfo();
            Model = model;
        }
    }
}
