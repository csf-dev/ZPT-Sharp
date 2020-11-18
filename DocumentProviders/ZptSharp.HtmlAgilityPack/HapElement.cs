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
                var newElement = new HapElement(native, (HapDocument)Document, parent, element.SourceInfo, new List<INode>());
                closedList.Add(native, newElement);
                if (parent != null) parent.sourceChildElements.Add(newElement);
                openList.AddRange(native.ChildNodes.Select((node, idx) => ((HtmlNode, HapElement)?)(node, (HapElement)element.ChildNodes[idx])));
            }

            return closedList[copiedElement];
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
                .Select(x => new HapAttribute(x, this))
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                                          var attr = ((HapAttribute)add.Item).NativeAttribute;
                                          NativeElement.SetAttributeValue(attr.OriginalName, attr.Value);
                                      },
                                      del => NativeElement.Attributes.Remove(del.Item.Name));

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

        IList<INode> GetSourceChildElements()
        {
            return NativeElement.ChildNodes
                .Select(x => new HapElement(x, (HapDocument)Doc, this, Source.CreateChild(x.Line)))
                .Cast<INode>()
                .ToList();
        }

        private HapElement(HtmlNode element,
                           HapDocument document,
                           INode parent,
                           IElementSourceInfo sourceInfo,
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
                          IElementSourceInfo sourceInfo = null)
            : this(element, document, parent, sourceInfo, null) { }
    }
}
