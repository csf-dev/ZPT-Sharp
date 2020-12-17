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
    public class HapElement : ElementBase
    {
        internal const string CommentFormat = "<!--{0}-->";

        readonly IList<INode> sourceChildElements;
        readonly EventRaisingList<INode> childElements;
        readonly EventRaisingList<IAttribute> attributes;

        /// <summary>
        /// Gets the native HTML Agility Pack <see cref="HtmlNode"/> instance which
        /// acts as the basis for the current element.
        /// </summary>
        /// <value>The native HTML Agility Pack element object.</value>
        public HtmlNode NativeElement { get; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public override IList<IAttribute> Attributes => attributes;

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public override IList<INode> ChildNodes => childElements;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:ZptSharp.Dom.INode"/> is an element node.
        /// </summary>
        /// <value><c>true</c> if the current instance is an element; otherwise, <c>false</c>.</value>
        public override bool IsElement => NativeElement.NodeType == HtmlNodeType.Element;

        /// <summary>
        /// Gets a value which indicates whether or not the current element has "CData"-like behaviour.
        /// One example of this is a <c>&lt;textarea&gt;</c> element.  Its contents are not HTML-encoded.
        /// </summary>
        /// <value><c>true</c> if the current element is treated like a <c>CData</c> element; otherwise, <c>false</c>.</value>
        bool ShouldElementBeTreatedAsCData
        {
            get
            {
                if (NativeElement.NodeType != HtmlNodeType.Element)
                    return false;

                return (from flag in HtmlNode.ElementsFlags
                        let behaviour = flag.Value
                        let elementName = flag.Key
                        where
                            behaviour.HasFlag(HtmlElementFlag.CData)
                            && String.Equals(elementName, NativeElement.Name, StringComparison.InvariantCultureIgnoreCase)
                        select elementName)
                    .Any();
            }
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="HapElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="HapElement"/>.</returns>
        public override string ToString()
        {
            var attribs = NativeElement.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attribs.Count > 0;

            return $"<{NativeElement.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attribs)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override INode GetCopy()
        {
            var copiedElement = NativeElement.Clone();

            var closedList = new Dictionary<HtmlNode,HapElement>();
            (HtmlNode, HapElement)? current;
            for (var openList = new List<(HtmlNode, HapElement)?> { (copiedElement, this) };
                 (current = openList.FirstOrDefault()) != null;
                 openList.RemoveAt(0))
            {
                var (native, element) = current.Value;
                var parent = ReferenceEquals(element, this) ? null : closedList[native.ParentNode];
                var newElement = new HapElement(native, (HapDocument)Document, parent, element.SourceInfo, new List<INode>())
                {
                    PreReplacementSourceInfo = element.PreReplacementSourceInfo,
                };
                closedList.Add(native, newElement);
                if (parent != null) parent.sourceChildElements.Add(newElement);
                openList.AddRange(native.ChildNodes.Select((node, idx) => ((HtmlNode, HapElement)?)(node, (HapElement)element.ChildNodes[idx])));
            }

            return closedList[copiedElement];
        }

        /// <summary>
        /// Creates and returns a new comment node.
        /// </summary>
        /// <returns>The comment node.</returns>
        /// <param name="commentText">The text for the comment.</param>
        public override INode CreateComment(string commentText)
        {
            commentText = commentText ?? String.Empty;
            var node = NativeElement.OwnerDocument.CreateComment(String.Format(CommentFormat, commentText));
            return new HapElement(node, (HapDocument) Document);
        }

        /// <summary>
        /// <para>
        /// Creates and returns a new text node from the specified content.
        /// Even if the content contains valid markup, it is strictly to be treated as text.
        /// </para>
        /// <para>
        /// The node will be created in such a way as to be suitable for inclusion within
        /// the current element.  That means that if the current element is CDATA, then the text
        /// will not be HTML encoded, otherwise it will.
        /// </para>
        /// </summary>
        /// <returns>A text node.</returns>
        /// <param name="content">The text content for the node.</param>
        public override INode CreateTextNode(string content)
        {
            content = content ?? String.Empty;
            var text = ShouldElementBeTreatedAsCData ? content : HtmlEntity.Entitize(content, true, true);
            return new HapElement(NativeElement.OwnerDocument.CreateTextNode(text), (HapDocument)Document);
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
                .Select(x => (INode) new HapElement(x, (HapDocument)Document))
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
            return new HapAttribute(NativeElement.OwnerDocument.CreateAttribute(name));
        }

        /// <summary>
        /// Gets a value which indicates whether or not the current element is in the specified namespace.
        /// </summary>
        /// <returns><c>true</c>, if the element is in the specified namespace, <c>false</c> otherwise.</returns>
        /// <param name="namespace">A namespace.</param>
        public override bool IsInNamespace(Namespace @namespace)
        {
            if (@namespace == null)
                throw new ArgumentNullException(nameof(@namespace));
            if (!IsElement) return false;

            var nameParts = NativeElement.Name.Split(new [] { ':' }, 2);
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
        /// in the native HAP element.
        /// </para>
        /// </summary>
        /// <returns>The attributes collection.</returns>
        EventRaisingList<IAttribute> GetAttributesCollection()
        {
            var sourceAttributes = NativeElement.Attributes
                .Select(x => new HapAttribute(x) { Element = this })
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                                          var attr = ((HapAttribute)add.Item).NativeAttribute;
                                          NativeElement.SetAttributeValue(attr.OriginalName, attr.Value);
                                          add.Item.Element = this;
                                      },
                                      del => {
                                          NativeElement.Attributes.Remove(del.Item.Name);
                                          del.Item.Element = null;
                                      });

            return attribs;
        }

        /// <summary>
        /// Gets a list of <see cref="INode"/> which is wrapped in a list adapter that reacts to events.
        /// </summary>
        /// <param name="elements">The source list of child elements.</param>
        /// <returns>The child elements collection.</returns>
        EventRaisingList<INode> WrapChildElementsCollectionWithEvents(IList<INode> elements)
        {
            if (elements == null)
                throw new ArgumentNullException(nameof(elements));

            var eventBasedListWrapper = new EventRaisingList<INode>(elements);

            eventBasedListWrapper.SetupAfterActions(
                add => {
                    var index = ((IList<INode>)add.Collection).IndexOf(add.Item);
                    var item = (HapElement)add.Item;
                    var ele = item.NativeElement;

                    if (index >= NativeElement.ChildNodes.Count)
                        NativeElement.AppendChild(ele);
                    else
                        NativeElement.InsertBefore(ele, NativeElement.ChildNodes[index]);

                    item.IsImportedNode = true;
                    item.ParentElement = this;
                },
                del => {
                    var ele = ((HapElement)del.Item).NativeElement;
                    NativeElement.RemoveChild(ele);
                    del.Item.ParentElement = null;
                });

            return eventBasedListWrapper;
        }

        int? GetEndTagLineNumber(HtmlNode node)
        {
            if (node == null) return null;

            var startTagLineNumber = node.Line;
            var lineBreakCount = node.OuterHtml.Count(x => x == '\n');

            return startTagLineNumber + lineBreakCount;
        }

        IList<INode> GetSourceChildElements()
        {
            return NativeElement.ChildNodes
                .Select(x => new HapElement(x, (HapDocument)Doc, this, Source.CreateChild(x.Line, GetEndTagLineNumber(x))))
                .Cast<INode>()
                .ToList();
        }

        private HapElement(HtmlNode element,
                           HapDocument document,
                           INode parent,
                           ElementSourceInfo sourceInfo,
                           IList<INode> childNodes) : base(document, parent, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
            attributes = GetAttributesCollection();
            sourceChildElements = childNodes ?? GetSourceChildElements();
            childElements = WrapChildElementsCollectionWithEvents(sourceChildElements);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HapElement"/> class.
        /// </summary>
        /// <param name="element">The native element object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">Source info.</param>
        public HapElement(HtmlNode element,
                          HapDocument document,
                          INode parent = null,
                          ElementSourceInfo sourceInfo = null)
            : this(element, document, parent, sourceInfo, null) { }
    }
}
