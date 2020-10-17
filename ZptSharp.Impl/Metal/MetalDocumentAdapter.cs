using System;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An adapter for an <see cref="IDocument"/> which allows it to be used as
    /// <see cref="IProvidesMacros"/> and <see cref="Expressions.IGetsNamedTalesValue"/>.
    /// </summary>
    public class MetalDocumentAdapter : IProvidesMacros, IDocument, Expressions.IGetsNamedTalesValue
    {
        const string Macros = "macros";

        readonly IDocument document;

        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        public IDocumentSourceInfo SourceInfo => document.SourceInfo;

        /// <summary>
        /// Gets a collection of all of the macros
        /// </summary>
        /// <returns>The macros.</returns>
        public IDictionary<string, MetalMacro> GetMacros()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        public IElement GetRootElement() => document.GetRootElement();

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>A boolean indicating whether a value was successfully retrieved or not.</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="value">Exposes the retrieved value if this method returns success.</param>
        public bool TryGetValue(string name, out object value)
        {
            if(String.Equals(name, Macros, StringComparison.InvariantCulture))
            {
                value = GetMacros();
                return true;
            }

            value = null;
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalDocumentAdapter"/> class.
        /// </summary>
        /// <param name="document">The document to be wrapped.</param>
        public MetalDocumentAdapter(IDocument document)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
