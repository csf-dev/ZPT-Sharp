using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="IElement"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class ElementBase : IElement
    {
        /// <summary>
        /// Gets the parent document for the current element.
        /// </summary>
        /// <value>The document.</value>
        public IDocument Document { get; }

        /// <summary>
        /// Gets information which indicates the original source of the element (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        public IElementSourceInfo SourceInfo { get; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public abstract IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public abstract IList<IElement> ChildElements { get; }

        IDocumentSourceInfo IHasDocumentSourceInfo.SourceInfo => SourceInfo.Document;

        IEnumerable<IElement> IHasElements.GetChildElements() => ChildElements;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementBase"/> class.
        /// </summary>
        /// <param name="document">The element's document.</param>
        /// <param name="sourceInfo">The element source info.</param>
        protected ElementBase(IDocument document,
                              IElementSourceInfo sourceInfo = null)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            SourceInfo = sourceInfo;
        }
    }
}
