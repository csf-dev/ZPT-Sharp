using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="INode"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class NodeBase : INode
    {
        /// <summary>
        /// A field for the <see cref="IDocument"/> to which this node belongs.
        /// </summary>
        protected readonly IDocument Doc;

        /// <summary>
        /// A field for the source information relating to this node.
        /// </summary>
        protected readonly NodeSourceInfo Source;

        INode parent;

        /// <summary>
        /// A field for whether or not the node is imported.
        /// </summary>
        protected bool IsImportedNode;

        /// <summary>
        /// Gets the parent document for the current node.
        /// </summary>
        /// <value>The document.</value>
        public virtual IDocument Document => Doc;

        /// <summary>
        /// Gets information which indicates the original source of the node (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        public virtual NodeSourceInfo SourceInfo => Source;

        /// <summary>
        /// Gets or sets information about the source of the node  (for example, a file path and line number) before it was replaced.
        /// For most nodes this will be <see langword="null"/>, but when the current node is a replacement (such as METAL macro usage),
        /// this property will contain source information for the replaced node.
        /// </summary>
        /// <value>The pre-replacement source info.</value>
        public virtual NodeSourceInfo PreReplacementSourceInfo { get; set; }

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
        /// Gets or sets the parent for the current node.  This will be a <see langword="null"/>
        /// reference if the current instance is the root node of the document or if the node
        /// is not attached to a DOM.
        /// </summary>
        /// <value>The parent node.</value>
        public virtual INode ParentNode
        {
            get => parent;
            set => parent = value;
        }

        /// <summary>
        /// Gets a collection of the node's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public abstract IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the nodes contained within the current node.
        /// </summary>
        /// <value>The child nodes.</value>
        public abstract IList<INode> ChildNodes { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:ZptSharp.Dom.INode"/> is an node node.
        /// </summary>
        /// <value><c>true</c> if the current instance is an node; otherwise, <c>false</c>.</value>
        public abstract bool IsElement { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="INode"/> is a text node.
        /// </summary>
        /// <value><c>true</c> if the current instance is a text node; otherwise, <c>false</c>.</value>
        public abstract bool IsTextNode { get; }

        /// <summary>
        /// Gets or sets the text content of a text node.  Returns <see langword="null"/> and throws an exception
        /// if the current node is not a text node.
        /// </summary>
        /// <seealso cref="IsTextNode"/>
        public abstract string Text { get; set; }

        /// <summary>
        /// Gets a copy of the current node and all of its children.
        /// </summary>
        /// <returns>The copied node.</returns>
        public abstract INode GetCopy();

        /// <summary>
        /// Gets a value which indicates whether or not the current node is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the node is in the specified namespace, <c>false</c> otherwise.</returns>
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

        IEnumerable<INode> IHasNodes.GetChildNodes() => ChildNodes;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodeBase"/> class.
        /// </summary>
        /// <param name="document">The node's document.</param>
        /// <param name="parent">The parent node.</param>
        /// <param name="sourceInfo">The node source info.</param>
        protected NodeBase(IDocument document,
                              INode parent = null,
                              NodeSourceInfo sourceInfo = null)
        {
            this.Doc = document ?? throw new ArgumentNullException(nameof(document));
            this.parent = parent;
            this.Source = sourceInfo ?? new NodeSourceInfo(new UnknownSourceInfo());
        }
    }
}
