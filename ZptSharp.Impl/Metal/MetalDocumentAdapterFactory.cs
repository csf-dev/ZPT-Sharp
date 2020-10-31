using System;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IGetsMetalDocumentAdapter"/> which uses dependency
    /// injection to backfill the other dependencies, as well as the document.
    /// </summary>
    public class MetalDocumentAdapterFactory : IGetsMetalDocumentAdapter
    {
        readonly ISearchesForAttributes attributeSearcher;
        readonly IGetsMetalAttributeSpecs specProvider;

        /// <summary>
        /// Gets the METAL document adapter for the specified <paramref name="document"/>.
        /// </summary>
        /// <returns>The METAL document adapter.</returns>
        /// <param name="document">A document.</param>
        public MetalDocumentAdapter GetMetalDocumentAdapter(IDocument document)
        {
            return new MetalDocumentAdapter(document, attributeSearcher, specProvider);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalDocumentAdapterFactory"/> class.
        /// </summary>
        /// <param name="attributeSearcher">Attribute searcher.</param>
        /// <param name="specProvider">Spec provider.</param>
        public MetalDocumentAdapterFactory(ISearchesForAttributes attributeSearcher, IGetsMetalAttributeSpecs specProvider)
        {
            this.attributeSearcher = attributeSearcher ?? throw new ArgumentNullException(nameof(attributeSearcher));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
        }
    }
}
