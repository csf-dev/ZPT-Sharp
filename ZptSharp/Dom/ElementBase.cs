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
        readonly IDocument document;
        readonly IElementSourceInfo sourceInfo;

        /// <summary>
        /// Gets the parent document for the current element.
        /// </summary>
        /// <value>The document.</value>
        public virtual IDocument Document => document;

        /// <summary>
        /// Gets information which indicates the original source of the element (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        public virtual IElementSourceInfo SourceInfo => sourceInfo;

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
            this.document = document ?? throw new ArgumentNullException(nameof(document));
            this.sourceInfo = sourceInfo;
        }
    }
}
