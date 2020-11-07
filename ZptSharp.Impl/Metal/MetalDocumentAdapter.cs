using System;
using System.Collections.Generic;
using ZptSharp.Dom;
using System.Linq;
using System.Threading.Tasks;
using ZptSharp.Expressions;

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
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        public Task<GetValueResult> TryGetValueAsync(string name)
        {
            if (String.Equals(name, Macros, StringComparison.InvariantCulture))
                return Task.FromResult(GetValueResult.For(GetMacros()));

            return Task.FromResult(GetValueResult.Failure);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalDocumentAdapter"/> class.
        /// </summary>
        /// <param name="document">The document to be wrapped.</param>
        /// <param name="attributeSearcher">A service which will search for attributes.</param>
        /// <param name="specProvider">A METAL attribute spec provider.</param>
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
