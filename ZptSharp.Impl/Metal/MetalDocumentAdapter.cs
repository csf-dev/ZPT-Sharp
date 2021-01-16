using System;
using System.Collections.Generic;
using ZptSharp.Dom;
using System.Linq;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using System.Threading;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An adapter for an <see cref="IDocument"/> which allows it to be used as
    /// <see cref="IProvidesMacros"/> and <see cref="Expressions.IGetsNamedTalesValue"/>.
    /// </summary>
    public class MetalDocumentAdapter : IProvidesMacros, Expressions.IGetsNamedTalesValue
    {
        const string
            MacrosVar = "macros",
            SourceNameVar = "sourcename";

        readonly ISearchesForAttributes attributeSearcher;
        readonly IGetsMetalAttributeSpecs specProvider;

        /// <summary>
        /// Gets the document for the current instance.
        /// </summary>
        public IDocument Document { get; }

        /// <summary>
        /// Gets a collection of all of the macros
        /// </summary>
        /// <returns>The macros.</returns>
        public IDictionary<string, MetalMacro> GetMacros()
        {
            return attributeSearcher
                .SearchForAttributes(Document, specProvider.DefineMacro)
                .Select(a => new MetalMacro(a.Value, a.Node.GetCopy()))
                .ToDictionary(k => k.Name, v => v);
        }

        /// <summary>
        /// Gets the name of the source info for this template.
        /// </summary>
        /// <value>The name of the source.</value>
        public string SourceName => Document.SourceInfo.Name;

        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, CancellationToken cancellationToken = default)
        {
            if (String.Equals(name, MacrosVar, StringComparison.InvariantCulture))
                return Task.FromResult(GetValueResult.For(GetMacros()));

            if (String.Equals(name, SourceNameVar, StringComparison.InvariantCulture))
                return Task.FromResult(GetValueResult.For(SourceName));

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
            Document = document ?? throw new ArgumentNullException(nameof(document));
            this.attributeSearcher = attributeSearcher ?? throw new System.ArgumentNullException(nameof(attributeSearcher));
            this.specProvider = specProvider ?? throw new System.ArgumentNullException(nameof(specProvider));
        }
    }
}
