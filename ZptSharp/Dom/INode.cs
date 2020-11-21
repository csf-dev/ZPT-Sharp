using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM node.
    /// </summary>
    public interface INode : IHasDocumentSourceInfo, IHasElements
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
        new ElementSourceInfo SourceInfo { get; }

        /// <summary>
        /// Gets or sets information about the source of the element  (for example, a file path and line number) before it was replaced.
        /// For most elements this will be <see langword="null"/>, but when the current element is a replacement (such as METAL macro usage),
        /// this property will contain source information for the replaced element.
        /// </summary>
        /// <value>The pre-replacement source info.</value>
        ElementSourceInfo PreReplacementSourceInfo { get; set; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the children contained within the current node.
        /// </summary>
        /// <value>The child elements.</value>
        IList<INode> ChildNodes { get; }

        /// <summary>
        /// Gets or sets the parent for the current element.  This will be a <see langword="null"/>
        /// reference if the current instance is the root element of the document or if the element
        /// is not attached to a DOM.
        /// </summary>
        /// <value>The parent element.</value>
        INode ParentElement { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="INode"/> is an element node.
        /// </summary>
        /// <value><c>true</c> if the current instance is an element; otherwise, <c>false</c>.</value>
        bool IsElement { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="INode"/> is imported.  A node is imported
        /// if its associated <see cref="Document"/> differs from the document upon its <see cref="ParentElement"/>.
        /// </summary>
        /// <value><c>true</c> if this node is imported; otherwise, <c>false</c>.</value>
        bool IsImported { get; }

        /// <summary>
        /// Replaces the specified child element (the <paramref name="toReplace"/> parameter)
        /// using the specified <paramref name="replacement"/> element.
        /// Note that this means that the current element will be detached/removed from its parent as a side-effect.
        /// Further DOM manipulation should occur using the replacement element and not the replaced element.
        /// </summary>
        /// <param name="toReplace">The child element to replace.</param>
        /// <param name="replacement">The replacement element.</param>
        void ReplaceChild(INode toReplace, INode replacement);

        /// <summary>
        /// Removes the current element from the DOM but preserves all of its children.
        /// Essentially this replaces the current element (on its parent) with the element's children.
        /// </summary>
        void Omit();

        /// <summary>
        /// Gets a value which indicates whether or not the current element is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the element is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">A namespace.</param>
        bool IsInNamespace(Namespace @namespace);

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        INode GetCopy();
    }
}
