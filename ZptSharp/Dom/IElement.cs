using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM element.
    /// </summary>
    public interface IElement : IHasDocumentSourceInfo, IHasElements
    {
        /// <summary>
        /// Gets the parent document for the current element.
        /// </summary>
        /// <value>The document.</value>
        IDocument Document { get; }

        /// <summary>
        /// Gets information which indicates the original source of the element (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        new IElementSourceInfo SourceInfo { get; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        IList<IElement> ChildElements { get; }

        /// <summary>
        /// Gets or sets the parent for the current element.  This will be a <see langword="null"/>
        /// reference if the current instance is the root element of the document or if the element
        /// is not attached to a DOM.
        /// </summary>
        /// <value>The parent element.</value>
        IElement ParentElement { get; set; }

        /// <summary>
        /// Replaces the specified child element (the <paramref name="toReplace"/> parameter)
        /// using the specified <paramref name="replacement"/> element.
        /// Note that this means that the current element will be detached/removed from its parent as a side-effect.
        /// Further DOM manipulation should occur using the replacement element and not the replaced element.
        /// </summary>
        /// <param name="toReplace">The child element to replace.</param>
        /// <param name="replacement">The replacement element.</param>
        void ReplaceChild(IElement toReplace, IElement replacement);
    }
}
