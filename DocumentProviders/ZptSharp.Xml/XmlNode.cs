using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using CSF.Collections.EventRaising;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="INode"/> which is based upon an XML <see cref="XNode"/>.
    /// </summary>
    public class XmlNode : NodeBase
    {
        readonly IList<INode> sourceChildNodes;
        readonly EventRaisingList<INode> childNodes;
        readonly EventRaisingList<IAttribute> attributes;

                /// <summary>
        /// Gets the native XML <see cref="XNode"/> instance which
        /// acts as the basis for the current node.
        /// </summary>
        /// <value>The native XML node object.</value>
        public XNode NativeNode { get; }

        /// <summary>
        /// Gets a representation of <see cref="NativeNode"/> as an <see cref="XElement"/>,
        /// if it is in fact an element node.
        /// </summary>
        /// <value>The element node.</value>
        protected XElement ElementNode => NativeNode as XElement;

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
        public override bool IsElement => NativeNode.NodeType == XmlNodeType.Element;

        /// <summary>
        /// Gets a value indicating whether this <see cref="INode"/> is a text node.
        /// </summary>
        /// <value><c>true</c> if the current instance is a text node; otherwise, <c>false</c>.</value>
        public override bool IsTextNode => NativeNode.NodeType == XmlNodeType.Text;

        /// <summary>
        /// Gets or sets the text content of a text node.  Returns <see langword="null"/> and throws an exception
        /// if the current node is not a text node.
        /// </summary>
        /// <seealso cref="IsTextNode"/>
        public override string Text
        {
            get => IsTextNode ? ((XText)NativeNode).Value : null;
            set => {
                if (!IsTextNode) throw new InvalidOperationException("This operation is not allowed on non-text nodes.");
                ((XText)NativeNode).Value = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="XmlNode"/>.  If it is an element node then this method shows the element's start-tag.
        /// Otherwise it returns the same as the native <see cref="XNode"/>'s <see cref="Object.ToString()"/> method.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="XmlNode"/>.</returns>
        public override string ToString()
        {
            if (!IsElement) return NativeNode.ToString();

            var attribs = ElementNode.Attributes()
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attribs.Count > 0;

            return $"<{ElementNode.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attribs)}>";
        }

        /// <summary>
        /// Gets a copy of the current node and all of its children.
        /// </summary>
        /// <returns>The copied node.</returns>
        public override INode GetCopy()
        {
            var copiedNode = new XElement(ElementNode);

            var closedList = new Dictionary<XNode, XmlNode>();
            (XNode, XmlNode)? current;
            for (var openList = new List<(XNode, XmlNode)?> { (copiedNode, this) };
                 (current = openList.FirstOrDefault()) != null;
                 openList.RemoveAt(0))
            {
                var (native, node) = current.Value;
                var parent = ReferenceEquals(node, this) ? null : closedList[native.Parent];
                var newNode = new XmlNode(native, (XmlDocument)Document, parent, node.SourceInfo, new List<INode>())
                {
                    PreReplacementSourceInfo = node.PreReplacementSourceInfo,
                };
                closedList.Add(native, newNode);
                if (parent != null) parent.sourceChildNodes.Add(newNode);
                if(native is XElement nativeNode)
                    openList.AddRange(nativeNode.Nodes().Select((n, idx) => ((XNode, XmlNode)?)(n, (XmlNode)node.ChildNodes[idx])));
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
            if (commentText == null)
                throw new ArgumentNullException(nameof(commentText));

            var node = new XComment(commentText);
            return new XmlNode(node, (XmlDocument) Document);
        }

        /// <summary>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public override INode CreateTextNode(string content)
            => new XmlNode(new XText(content ?? String.Empty), (XmlDocument) Document);

        /// <summary>
        /// Parses the specified text <paramref name="markup"/> and returns the resulting nodes.
        /// </summary>
        /// <returns>The parsed nodes.</returns>
        /// <param name="markup">Markup text.</param>
        public override IList<INode> ParseAsNodes(string markup)
            => new[] { new XmlNode(XElement.Parse(markup ?? String.Empty), (XmlDocument)Document) };

        /// <summary>
        /// Creates and returns a new attribute from the specified specification.
        /// </summary>
        /// <returns>An attribute.</returns>
        /// <param name="spec">The attribute specification which will be used to name the attribute.</param>
        public override IAttribute CreateAttribute(AttributeSpec spec)
        {
            var name = XName.Get(spec.Name, spec.Namespace?.Uri ?? XNamespace.None.NamespaceName);
            return new XmlAttribute(new XAttribute(name, String.Empty));
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

            return String.Equals(ElementNode.Name.NamespaceName, @namespace.Uri, StringComparison.InvariantCulture);
        }

        /// <summary>
        /// <para>
        /// Called by the constructor; initialises and returns a <see cref="EventRaisingList{IAttribute}"/>
        /// for use as the <see cref="Attributes"/> collection.
        /// </para>
        /// <para>
        /// This event-raising list is used to keep the attributes collection in-sync with the attributes
        /// in the native XML node.
        /// </para>
        /// </summary>
        /// <returns>The attributes collection.</returns>
        EventRaisingList<IAttribute> GetAttributesCollection()
        {
            if (!IsElement) return new EventRaisingList<IAttribute>(new List<IAttribute>());

            var sourceAttributes = ElementNode.Attributes()
                .Select(x => new XmlAttribute(x) { Node = this })
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                                          var xmlAttr = (XmlAttribute) add.Item;
                                          var attr = xmlAttr.NativeAttribute;
                                          ElementNode.SetAttributeValue(attr.Name, attr.Value);
                                          // Because we didn't directly use the native attribute, we essentially
                                          // just created a new one.  That means we need to re-set the native
                                          // attribute on the ZPT attribute object by way of a fixup.
                                          xmlAttr.NativeAttribute = ElementNode.Attribute(attr.Name);
                                          add.Item.Node = this;
                                      },
                                      del => {
                                          ((XmlAttribute)del.Item).NativeAttribute.Remove();
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
                    var item = (XmlNode)add.Item;
                    var ele = item.NativeNode;

                    if (index >= ElementNode.Nodes().Count())
                        ElementNode.Add(ele);
                    else
                    {
                        var toInsertBefore = ElementNode.Nodes().Skip(index).First();
                        toInsertBefore.AddBeforeSelf(ele);
                    }

                    item.IsImportedNode = true;
                    item.ParentNode = this;
                },
                del => {
                    var ele = ((XmlNode)del.Item).NativeNode;
                    ele.Remove();
                    del.Item.ParentNode = null;
                });

            return eventBasedListWrapper;
        }

        IList<INode> GetSourceChildNodes()
        {
            if (!IsElement) return new INode[0];

            return ElementNode.Nodes()
                .Select(x => new XmlNode(x, (XmlDocument)Doc, this, Source.CreateChild(GetLineNumber(x))))
                .Cast<INode>()
                .ToList();
        }

        /// <summary>
        /// Gets the line number for the specified node.
        /// </summary>
        /// <returns>The line number.</returns>
        /// <param name="node">The node for which to get a line number.</param>
        internal static int? GetLineNumber(XNode node)
            => ((node is IXmlLineInfo lineInfo) && lineInfo.HasLineInfo()) ? lineInfo.LineNumber : (int?) null;

        private XmlNode(XNode node,
                           XmlDocument document,
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
        /// Initializes a new instance of the <see cref="XmlNode"/> class.
        /// </summary>
        /// <param name="node">The native node object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent node.</param>
        /// <param name="sourceInfo">Source info.</param>
        public XmlNode(XNode node,
                          XmlDocument document,
                          INode parent = null,
                          NodeSourceInfo sourceInfo = null)
            : this(node, document, parent, sourceInfo, null) { }
    }
}