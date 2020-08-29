using System;
using System.IO;
using ZptSharp.Config;

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
        /// Gets a configuration object to be used for the rendering process.
        /// </summary>
        /// <value>The configuration.</value>
        public RenderingConfig Config { get; }

        /// <summary>
        /// An action which is used to build &amp; add values to the root ZPT context.
        /// </summary>
        /// <value>The context builder.</value>
        public Action<IConfiguresRootContext> ContextBuilder { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderZptDocumentRequest"/> class.
        /// </summary>
        /// <param name="document">The document stream.</param>
        /// <param name="model">The model.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="contextBuilder">The context builder action.</param>
        public RenderZptDocumentRequest(Stream document, object model, RenderingConfig config, Action<IConfiguresRootContext> contextBuilder)
        {
            DocumentStream = document ?? throw new System.ArgumentNullException(nameof(document));
            Model = model;
            Config = config ?? throw new System.ArgumentNullException(nameof(config));
            ContextBuilder = contextBuilder ?? throw new ArgumentNullException(nameof(contextBuilder));
        }
    }
}
