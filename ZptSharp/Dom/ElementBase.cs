using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="INode"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class ElementBase : INode
    {
        /// <summary>
        /// A field for the <see cref="IDocument"/> to which this element belongs.
        /// </summary>
        protected readonly IDocument Doc;

        /// <summary>
        /// A field for the source information relating to this element.
        /// </summary>
        protected readonly ElementSourceInfo Source;

        INode Parent;

        /// <summary>
        /// A field for whether or not the element is imported.
        /// </summary>
        protected bool IsImportedNode;

        /// <summary>
        /// Gets the parent document for the current element.
        /// </summary>
        /// <value>The document.</value>
        public virtual IDocument Document => Doc;

        /// <summary>
        /// Gets information which indicates the original source of the element (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        public virtual ElementSourceInfo SourceInfo => Source;

        /// <summary>
        /// Gets or sets information about the source of the element  (for example, a file path and line number) before it was replaced.
        /// For most elements this will be <see langword="null"/>, but when the current element is a replacement (such as METAL macro usage),
        /// this property will contain source information for the replaced element.
        /// </summary>
        /// <value>The pre-replacement source info.</value>
        public virtual ElementSourceInfo PreReplacementSourceInfo { get; set; }

        /// <summary>
        /// <para>
        /// Gets a value indicating whether this <see cref="INode"/> is imported.  A node is imported
        /// if it was not a part of the document, as the document was originally loaded.
        /// </para>
        /// <para>
        /// For example, a node included via a METAL macro or TAL content/replace directive is imported.
        /// </para>
        /// </summary>
        /// <value><c>true</c> if this node is imported; otherwise, <c>false</c>.</value>
        public virtual bool IsImported => IsImportedNode;

        /// <summary>
        /// Gets or sets the parent for the current element.  This will be a <see langword="null"/>
        /// reference if the current instance is the root element of the document or if the element
        /// is not attached to a DOM.
        /// </summary>
        /// <value>The parent element.</value>
        public virtual INode ParentElement
        {
            get => Parent;
            set => Parent = value;
        }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public abstract IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public abstract IList<INode> ChildNodes { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:ZptSharp.Dom.INode"/> is an element node.
        /// </summary>
        /// <value><c>true</c> if the current instance is an element; otherwise, <c>false</c>.</value>
        public abstract bool IsElement { get; }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public abstract INode GetCopy();

        /// <summary>
        /// Gets a value which indicates whether or not the current element is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the element is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">A namespace.</param>
        public abstract bool IsInNamespace(Namespace @namespace);

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        public abstract INode CreateComment(string commentText);

        /// <summary>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public abstract INode CreateTextNode(string content);

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        public abstract IList<INode> ParseAsNodes(string markup);

        /// <summary>
        /// Creates and returns a new attribute from the specified specification.
        /// </summary>
        /// <returns>An attribute.</returns>
        /// <param name="spec">The attribute specification which will be used to name the attribute.</param>
        public abstract IAttribute CreateAttribute(AttributeSpec spec);

        IDocumentSourceInfo IHasDocumentSourceInfo.SourceInfo => SourceInfo.Document;

        IEnumerable<INode> IHasElements.GetChildElements() => ChildNodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementBase"/> class.
        /// </summary>
        /// <param name="document">The element's document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">The element source info.</param>
        protected ElementBase(IDocument document,
                              INode parent = null,
                              ElementSourceInfo sourceInfo = null)
        {
            this.Doc = document ?? throw new ArgumentNullException(nameof(document));
            this.Parent = parent;
            this.Source = sourceInfo ?? new ElementSourceInfo(new UnknownSourceInfo());
        }
    }
}
