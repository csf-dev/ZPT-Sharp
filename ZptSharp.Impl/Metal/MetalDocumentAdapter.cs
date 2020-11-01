using System;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Rendering;
using System.Linq;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An adapter for an <see cref="IDocument"/> which allows it to be used as
    /// <see cref="IProvidesMacros"/> and <see cref="Expressions.IGetsNamedTalesValue"/>.
    /// </summary>
    public class MetalDocumentAdapter : IProvidesMacros, Expressions.IGetsNamedTalesValue
    {
        const string Macros = "macros";

        readonly IDocument document;
        readonly ISearchesForAttributes attributeSearcher;
        readonly IGetsMetalAttributeSpecs specProvider;

        /// <summary>
        /// Gets a collection of all of the macros
        /// </summary>
        /// <returns>The macros.</returns>
        public IDictionary<string, MetalMacro> GetMacros()
        {
            return attributeSearcher
                .SearchForAttributes(document, specProvider.DefineMacro)
                .Select(a => new MetalMacro(a.Value, a.Element))
                .ToDictionary(k => k.Name, v => v);
        }

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
        /// <param name="attributeSearcher">A service which will search for attributes.</param>
        public MetalDocumentAdapter(IDocument document,
                                    ISearchesForAttributes attributeSearcher,
                                    IGetsMetalAttributeSpecs specProvider)
        {
            this.document = document ?? throw new ArgumentNullException(nameof(document));
            this.attributeSearcher = attributeSearcher ?? throw new System.ArgumentNullException(nameof(attributeSearcher));
            this.specProvider = specProvider ?? throw new System.ArgumentNullException(nameof(specProvider));
        }
    }
}
