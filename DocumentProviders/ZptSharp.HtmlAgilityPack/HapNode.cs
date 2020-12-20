using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Collections.EventRaising;
using HtmlAgilityPack;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="INode"/> which is based upon an HTML Agility Pack <see cref="HtmlNode"/>.
    /// </summary>
    public class HapNode : NodeBase
    {
        internal const string CommentFormat = "<!--{0}-->";

        readonly IList<INode> sourceChildNodes;
        readonly EventRaisingList<INode> childNodes;
        readonly EventRaisingList<IAttribute> attributes;

        /// <summary>
        /// Gets the native HTML Agility Pack <see cref="HtmlNode"/> instance which
        /// acts as the basis for the current node.
        /// </summary>
        /// <value>The native HTML Agility Pack node object.</value>
        public HtmlNode NativeNode { get; }

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
        public override bool IsElement => NativeNode.NodeType == HtmlNodeType.Element;

        /// <summary>
        /// Gets a value which indicates whether or not the current element node has "CData"-like behaviour.
        /// One example of this is a <c>&lt;textarea&gt;</c> node.  Its contents are not HTML-encoded.
        /// </summary>
        /// <value><c>true</c> if the current element is treated like a <c>CData</c> element; otherwise, <c>false</c>.</value>
        bool ShouldNodeBeTreatedAsCData
        {
            get
            {
                if (!IsElement) return false;

                return (from flag in HtmlNode.ElementsFlags
                        let behaviour = flag.Value
                        let elementName = flag.Key
                        where
                            behaviour.HasFlag(HtmlElementFlag.CData)
                            && String.Equals(elementName, NativeNode.Name, StringComparison.InvariantCultureIgnoreCase)
                        select elementName)
                    .Any();
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="HapNode"/>.  If it is an element node then this method shows the element's start-tag.
        /// Otherwise it returns the same as the native <see cref="HtmlNode"/>'s <see cref="Object.ToString()"/> method.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="HapNode"/>.</returns>
        public override string ToString()
        {
            if (!IsElement) return NativeNode.ToString();

            var attribs = NativeNode.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attribs.Count > 0;

            return $"<{NativeNode.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attribs)}>";
        }

        /// <summary>
        /// Gets a copy of the current node and all of its children.
        /// </summary>
        /// <returns>The copied node.</returns>
        public override INode GetCopy()
        {
            var copiedNode = NativeNode.Clone();

            var closedList = new Dictionary<HtmlNode,HapNode>();
            (HtmlNode, HapNode)? current;
            for (var openList = new List<(HtmlNode, HapNode)?> { (copiedNode, this) };
                 (current = openList.FirstOrDefault()) != null;
                 openList.RemoveAt(0))
            {
                var (native, node) = current.Value;
                var parent = ReferenceEquals(node, this) ? null : closedList[native.ParentNode];
                var newNode = new HapNode(native, (HapDocument)Document, parent, node.SourceInfo, new List<INode>())
                {
                    PreReplacementSourceInfo = node.PreReplacementSourceInfo,
                };
                closedList.Add(native, newNode);
                if (parent != null) parent.sourceChildNodes.Add(newNode);
                openList.AddRange(native.ChildNodes.Select((n, idx) => ((HtmlNode, HapNode)?)(n, (HapNode)node.ChildNodes[idx])));
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
            var node = NativeNode.OwnerDocument.CreateComment(String.Format(CommentFormat, commentText));
            return new HapNode(node, (HapDocument) Document);
        }

        /// <summary>
        /// <para>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </para>
        /// <para>
        /// The node will be created in such a way as to be suitable for inclusion within
        /// the current node.  That means that if the current node is CDATA, then the text
        /// will not be HTML encoded, otherwise it will.
        /// </para>
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public override INode CreateTextNode(string content)
        {
            content = content ?? String.Empty;
            var text = ShouldNodeBeTreatedAsCData ? content : HtmlEntity.Entitize(content, true, true);
            return new HapNode(NativeNode.OwnerDocument.CreateTextNode(text), (HapDocument)Document);
        }

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        public override IList<INode> ParseAsNodes(string markup)
        {
            if (markup == null) return new INode[0];

            var doc = new HtmlDocument();
            doc.LoadHtml(markup);

            return doc.DocumentNode.ChildNodes
                .Select(x => (INode) new HapNode(x, (HapDocument)Document))
                .ToList();
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
            return new HapAttribute(NativeNode.OwnerDocument.CreateAttribute(name));
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

            var nameParts = NativeNode.Name.Split(new [] { ':' }, 2);
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
        /// in the native HAP node.
        /// </para>
        /// </summary>
        /// <returns>The attributes collection.</returns>
        EventRaisingList<IAttribute> GetAttributesCollection()
        {
            var sourceAttributes = NativeNode.Attributes
                .Select(x => new HapAttribute(x) { Node = this })
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                                          var attr = ((HapAttribute)add.Item).NativeAttribute;
                                          NativeNode.SetAttributeValue(attr.OriginalName, attr.Value);
                                          add.Item.Node = this;
                                      },
                                      del => {
                                          NativeNode.Attributes.Remove(del.Item.Name);
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
                    var item = (HapNode)add.Item;
                    var ele = item.NativeNode;

                    if (index >= NativeNode.ChildNodes.Count)
                        NativeNode.AppendChild(ele);
                    else
                        NativeNode.InsertBefore(ele, NativeNode.ChildNodes[index]);

                    item.IsImportedNode = true;
                    item.ParentNode = this;
                },
                del => {
                    var ele = ((HapNode)del.Item).NativeNode;
                    NativeNode.RemoveChild(ele);
                    del.Item.ParentNode = null;
                });

            return eventBasedListWrapper;
        }

        static int? GetEndTagLineNumber(HtmlNode node)
        {
            if (node == null) return null;

            var startTagLineNumber = node.Line;
            var lineBreakCount = node.OuterHtml.Count(x => x == '\n');

            return startTagLineNumber + lineBreakCount;
        }

        IList<INode> GetSourceChildNodes()
        {
            return NativeNode.ChildNodes
                .Select(x => new HapNode(x, (HapDocument)Doc, this, Source.CreateChild(x.Line, GetEndTagLineNumber(x))))
                .Cast<INode>()
                .ToList();
        }

        private HapNode(HtmlNode node,
                           HapDocument document,
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
        /// Initializes a new instance of the <see cref="HapNode"/> class.
        /// </summary>
        /// <param name="node">The native node object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent node.</param>
        /// <param name="sourceInfo">Source info.</param>
        public HapNode(HtmlNode node,
                          HapDocument document,
                          INode parent = null,
                          NodeSourceInfo sourceInfo = null)
            : this(node, document, parent, sourceInfo, null) { }
    }
}
