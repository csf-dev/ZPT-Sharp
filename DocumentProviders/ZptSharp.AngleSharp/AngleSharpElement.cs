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
    /// Implementation of <see cref="INode"/> which is based upon an AngleSharp <see cref="As.IElement"/>.
    /// </summary>
    public class AngleSharpElement : ElementBase
    {
        readonly IList<INode> sourceChildElements;
        readonly EventRaisingList<IAttribute> attributes;
        readonly EventRaisingList<INode> childElements;

        /// <summary>
        /// Gets the native AngleSharp <see cref="As.INode"/> instance which
        /// acts as the basis for the current element.
        /// </summary>
        /// <value>The native AngleSharp element object.</value>
        public As.INode NativeElement { get; }

        /// <summary>
        /// Gets a representation of <see cref="NativeElement"/> as an <see cref="As.IElement"/>,
        /// if it is in fact an element node.
        /// </summary>
        /// <value>The element node.</value>
        protected As.IElement ElementNode
            => NativeElement as As.IElement;

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
        public override bool IsElement => NativeElement.NodeType == As.NodeType.Element;

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="AngleSharpElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="AngleSharpElement"/>.</returns>
        public override string ToString()
        {
            if (!IsElement) return NativeElement.ToString();

            var attrs = ElementNode.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attrs.Count > 0;

            return $"<{ElementNode.LocalName}{(hasAttributes? " " : String.Empty)}{String.Join(" ", attrs)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override INode GetCopy()
        {
            var copiedElement = NativeElement.Clone();

            var closedList = new Dictionary<As.INode, AngleSharpElement>();
            (As.INode, AngleSharpElement)? current;
            for (var openList = new List<(As.INode, AngleSharpElement)?> { (copiedElement, this) };
                 (current = openList.FirstOrDefault()) != null;
                 openList.RemoveAt(0))
            {
                var (native, element) = current.Value;
                var parent = ReferenceEquals(element, this) ? null : closedList[native.Parent];
                var newElement = new AngleSharpElement(native, (AngleSharpDocument)Document, parent, element.SourceInfo, new List<INode>())
                {
                    PreReplacementSourceInfo = element.PreReplacementSourceInfo,
                };
                closedList.Add(native, newElement);
                if (parent != null) parent.sourceChildElements.Add(newElement);
                openList.AddRange(native.ChildNodes.Select((node, idx) => ((As.INode, AngleSharpElement)?)(node, (AngleSharpElement)element.ChildNodes[idx])));
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
            var node = NativeElement.Owner.CreateComment(commentText);
            return new AngleSharpElement(node, (AngleSharpDocument) Document);
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
            return new AngleSharpElement(NativeElement.Owner.CreateTextNode(text), (AngleSharpDocument)Document);
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
            return nativeNodes.Select(x => new AngleSharpElement(x, (AngleSharpDocument)Document)).Cast<INode>().ToList();

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
            return new AngleSharpAttribute(NativeElement.Owner.CreateAttribute(name));
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
        /// in the native AngleSharp element.
        /// </para>
        /// </summary>
        /// <returns>The attributes collection.</returns>
        EventRaisingList<IAttribute> GetAttributesCollection()
        {
            if (!IsElement) return new EventRaisingList<IAttribute>(new List<IAttribute>());

            var sourceAttributes = ElementNode.Attributes
                .Select(x => new AngleSharpAttribute(x) { Element = this })
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                var attr = ((AngleSharpAttribute)add.Item).NativeAttribute;
                ElementNode.Attributes.SetNamedItem(((AngleSharpAttribute)add.Item).NativeAttribute);
                add.Item.Element = this;
            },
                                      del => {
                                          ElementNode.Attributes.RemoveNamedItem(del.Item.Name);
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
                    var item = (AngleSharpElement)add.Item;
                    var ele = item.NativeElement;

                    if (index >= NativeElement.ChildNodes.Count())
                        NativeElement.AppendChild(ele);
                    else
                        NativeElement.InsertBefore(ele, NativeElement.ChildNodes[index]);

                    item.IsImportedNode = true;
                    item.ParentElement = this;
                },
                del => {
                    var ele = ((AngleSharpElement)del.Item).NativeElement;
                    NativeElement.RemoveChild(ele);
                    del.Item.ParentElement = null;
                });

            return eventBasedListWrapper;
        }

        IList<INode> GetSourceChildElements()
        {
            return NativeElement.ChildNodes
                .Select(CreateChildElement)
                .Cast<INode>()
                .ToList();
        }

        AngleSharpElement CreateChildElement(As.INode node)
        {
            var doc = (AngleSharpDocument) Doc;
            return new AngleSharpElement(node, doc, this, GetSourceInfo(node));
        }

        ElementSourceInfo GetSourceInfo(As.INode node)
        {
            if (!(node is As.IElement ele)) return Source.CreateChild();

            var startTagLine = ele.SourceReference?.Position.Line;
            if(!startTagLine.HasValue) return Source.CreateChild();
            var lineBreakCount = ele.OuterHtml.Count(x => x == '\n');
            var endTagLine = startTagLine.Value + lineBreakCount;

            return Source.CreateChild(startTagLine.Value, endTagLine);
        }

        private AngleSharpElement(As.INode element,
                                  AngleSharpDocument document,
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
        /// Initializes a new instance of the <see cref="AngleSharpDocument"/> class.
        /// </summary>
        /// <param name="element">The native element object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">Source info.</param>
        public AngleSharpElement(As.INode element,
                          AngleSharpDocument document,
                          INode parent = null,
                          ElementSourceInfo sourceInfo = null)
            : this(element, document, parent, sourceInfo, null) { }

    }
}
