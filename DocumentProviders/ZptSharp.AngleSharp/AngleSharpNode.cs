using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Html.Parser;
using CSF.Collections.EventRaising;
using ZptSharp.Rendering;
using As = AngleSharp.Dom;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="INode"/> which is based upon an AngleSharp <see cref="As.INode"/>.
    /// </summary>
    public class AngleSharpNode : NodeBase
    {
        readonly IList<INode> sourceChildNodes;
        readonly EventRaisingList<IAttribute> attributes;
        readonly EventRaisingList<INode> childNodes;

        /// <summary>
        /// Gets the native AngleSharp <see cref="As.INode"/> instance which
        /// acts as the basis for the current node.
        /// </summary>
        /// <value>The native AngleSharp node object.</value>
        public As.INode NativeNode { get; }

        /// <summary>
        /// Gets a representation of <see cref="NativeNode"/> as an <see cref="As.IElement"/>,
        /// if it is in fact an element node.
        /// </summary>
        /// <value>The element node.</value>
        protected As.IElement ElementNode => NativeNode as As.IElement;

        /// <summary>
        /// Gets a collection of the node's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public override IList<IAttribute> Attributes => attributes;

        /// <summary>
        /// Gets the nodes contained within the current node.
        /// </summary>
        /// <value>The child nodes.</value>
        public override IList<INode> ChildNodes => childNodes;

        /// <summary>
        /// Gets a value indicating whether this <see cref="INode"/> is an element node.
        /// </summary>
        /// <value><c>true</c> if the current instance is an element; otherwise, <c>false</c>.</value>
        public override bool IsElement => NativeNode.NodeType == As.NodeType.Element;

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="AngleSharpNode"/>.  If it is an element node then this method shows the element's start-tag.
        /// Otherwise it returns the same as the native <see cref="As.INode"/>'s <see cref="Object.ToString()"/> method.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="AngleSharpNode"/>.</returns>
        public override string ToString()
        {
            if (!IsElement) return NativeNode.ToString();

            var attrs = ElementNode.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attrs.Count > 0;

            return $"<{ElementNode.LocalName}{(hasAttributes? " " : String.Empty)}{String.Join(" ", attrs)}>";
        }

        /// <summary>
        /// Gets a copy of the current node and all of its children.
        /// </summary>
        /// <returns>The copied node.</returns>
        public override INode GetCopy()
        {
            var copiedNode = NativeNode.Clone();

            var closedList = new Dictionary<As.INode, AngleSharpNode>();
            (As.INode, AngleSharpNode)? current;
            for (var openList = new List<(As.INode, AngleSharpNode)?> { (copiedNode, this) };
                 (current = openList.FirstOrDefault()) != null;
                 openList.RemoveAt(0))
            {
                var (native, node) = current.Value;
                var parent = ReferenceEquals(node, this) ? null : closedList[native.Parent];
                var newNode = new AngleSharpNode(native, (AngleSharpDocument)Document, parent, node.SourceInfo, new List<INode>())
                {
                    PreReplacementSourceInfo = node.PreReplacementSourceInfo,
                };
                closedList.Add(native, newNode);
                if (parent != null) parent.sourceChildNodes.Add(newNode);
                openList.AddRange(native.ChildNodes.Select((n, idx) => ((As.INode, AngleSharpNode)?)(n, (AngleSharpNode)node.ChildNodes[idx])));
            }

            return closedList[copiedNode];
        }

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        public override INode CreateComment(string commentText)
        {
            commentText = commentText ?? String.Empty;
            var node = NativeNode.Owner.CreateComment(commentText);
            return new AngleSharpNode(node, (AngleSharpDocument) Document);
        }

        /// <summary>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public override INode CreateTextNode(string content)
        {
            content = content ?? String.Empty;
            var text = content;
            return new AngleSharpNode(NativeNode.Owner.CreateTextNode(text), (AngleSharpDocument)Document);
        }

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        public override IList<INode> ParseAsNodes(string markup)
        {
            var context = BrowsingContext.New();
            var parser = context.GetService<IHtmlParser>();
            var nativeNodes = parser.ParseFragment(markup, ElementNode);
            return nativeNodes.Select(x => new AngleSharpNode(x, (AngleSharpDocument)Document)).Cast<INode>().ToList();

        }

        /// <summary>
        /// Creates and returns a new attribute from the specified specification.
        /// </summary>
        /// <returns>An attribute.</returns>
        /// <param name="spec">The attribute specification which will be used to name the attribute.</param>
        public override IAttribute CreateAttribute(AttributeSpec spec)
        {
            if (spec == null)
                throw new ArgumentNullException(nameof(spec));

            var name = (spec.Namespace?.Prefix != null) ? $"{spec.Namespace.Prefix}:{spec.Name}" : spec.Name;
            return new AngleSharpAttribute(NativeNode.Owner.CreateAttribute(name));
        }

        /// <summary>
        /// Gets a value which indicates whether or not the current node is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the node is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">A namespace.</param>
        public override bool IsInNamespace(Namespace @namespace)
        {
            if (@namespace == null)
                throw new ArgumentNullException(nameof(@namespace));
            if (!IsElement) return false;

            var nameParts = ElementNode.LocalName.Split(new[] { ':' }, 2);
            if (nameParts.Length < 2 && String.IsNullOrEmpty(@namespace.Prefix))
                return true;
            if (nameParts.Length < 2)
                return false;

            return String.Equals(nameParts[0], @namespace.Prefix, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// <para>
        /// Called by the constructor; initialises and returns a <see cref="EventRaisingList{IAttribute}"/>
        /// for use as the <see cref="Attributes"/> collection.
        /// </para>
        /// <para>
        /// This event-raising list is used to keep the attributes collection in-sync with the attributes
        /// in the native AngleSharp node.
        /// </para>
        /// </summary>
        /// <returns>The attributes collection.</returns>
        EventRaisingList<IAttribute> GetAttributesCollection()
        {
            if (!IsElement) return new EventRaisingList<IAttribute>(new List<IAttribute>());

            var sourceAttributes = ElementNode.Attributes
                .Select(x => new AngleSharpAttribute(x) { Node = this })
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                var attr = ((AngleSharpAttribute)add.Item).NativeAttribute;
                ElementNode.Attributes.SetNamedItem(attr);
                add.Item.Node = this;
            },
                                      del => {
                                          ElementNode.Attributes.RemoveNamedItem(del.Item.Name);
                                          del.Item.Node = null;
                                      });

            return attribs;
        }

        /// <summary>
        /// Gets a list of <see cref="INode"/> which is wrapped in a list adapter that reacts to events.
        /// </summary>
        /// <param name="nodes">The source list of child nodes.</param>
        /// <returns>The child nodes collection.</returns>
        EventRaisingList<INode> WrapChildNodesCollectionWithEvents(IList<INode> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            var eventBasedListWrapper = new EventRaisingList<INode>(nodes);

            eventBasedListWrapper.SetupAfterActions(
                add => {
                    var index = ((IList<INode>)add.Collection).IndexOf(add.Item);
                    var item = (AngleSharpNode)add.Item;
                    var ele = item.NativeNode;

                    if (index >= NativeNode.ChildNodes.Count())
                        NativeNode.AppendChild(ele);
                    else
                        NativeNode.InsertBefore(ele, NativeNode.ChildNodes[index]);

                    item.IsImportedNode = true;
                    item.ParentNode = this;
                },
                del => {
                    var ele = ((AngleSharpNode)del.Item).NativeNode;
                    NativeNode.RemoveChild(ele);
                    del.Item.ParentNode = null;
                });

            return eventBasedListWrapper;
        }

        IList<INode> GetSourceChildNodes()
        {
            return NativeNode.ChildNodes
                .Select(CreateChildNode)
                .Cast<INode>()
                .ToList();
        }

        AngleSharpNode CreateChildNode(As.INode node)
        {
            var doc = (AngleSharpDocument) Doc;
            return new AngleSharpNode(node, doc, this, GetSourceInfo(node));
        }

        NodeSourceInfo GetSourceInfo(As.INode node)
        {
            if (!(node is As.IElement ele)) return Source.CreateChild();

            var startTagLine = ele.SourceReference?.Position.Line;
            if(!startTagLine.HasValue) return Source.CreateChild();
            var lineBreakCount = ele.OuterHtml.Count(x => x == '\n');
            var endTagLine = startTagLine.Value + lineBreakCount;

            return Source.CreateChild(startTagLine.Value, endTagLine);
        }

        private AngleSharpNode(As.INode node,
                                  AngleSharpDocument document,
                                  INode parent,
                                  NodeSourceInfo sourceInfo,
                                  IList<INode> childNodes) : base(document, parent, sourceInfo)
        {
            NativeNode = node ?? throw new ArgumentNullException(nameof(node));
            attributes = GetAttributesCollection();
            sourceChildNodes = childNodes ?? GetSourceChildNodes();
            this.childNodes = WrapChildNodesCollectionWithEvents(sourceChildNodes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleSharpDocument"/> class.
        /// </summary>
        /// <param name="node">The native node object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent node.</param>
        /// <param name="sourceInfo">Source info.</param>
        public AngleSharpNode(As.INode node,
                          AngleSharpDocument document,
                          INode parent = null,
                          NodeSourceInfo sourceInfo = null)
            : this(node, document, parent, sourceInfo, null) { }

    }
}
