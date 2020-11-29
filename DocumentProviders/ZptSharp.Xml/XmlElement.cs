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
    /// Implementation of <see cref="INode"/> which is based upon an XML <see cref="XElement"/>.
    /// </summary>
    public class XmlElement : ElementBase
    {
        readonly IList<INode> sourceChildElements;
        readonly EventRaisingList<INode> childElements;
        readonly EventRaisingList<IAttribute> attributes;

                /// <summary>
        /// Gets the native XML <see cref="XNode"/> instance which
        /// acts as the basis for the current element.
        /// </summary>
        /// <value>The native XML element object.</value>
        public XNode NativeElement { get; }

        /// <summary>
        /// Gets a representation of <see cref="NativeElement"/> as an <see cref="XElement"/>,
        /// if it is in fact an element node.
        /// </summary>
        /// <value>The element node.</value>
        protected XElement ElementNode
            => NativeElement as XElement;

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
        public override bool IsElement => NativeElement.NodeType == XmlNodeType.Element;

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="XmlElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="XmlElement"/>.</returns>
        public override string ToString()
        {
            if (!IsElement) return NativeElement.ToString();

            var attribs = ElementNode.Attributes()
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attribs.Count > 0;

            return $"<{ElementNode.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attribs)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override INode GetCopy()
        {
            var copiedElement = new XElement(ElementNode);

            var closedList = new Dictionary<XNode, XmlElement>();
            (XNode, XmlElement)? current;
            for (var openList = new List<(XNode, XmlElement)?> { (copiedElement, this) };
                 (current = openList.FirstOrDefault()) != null;
                 openList.RemoveAt(0))
            {
                var (native, element) = current.Value;
                var parent = ReferenceEquals(element, this) ? null : closedList[native.Parent];
                var newElement = new XmlElement(native, (XmlDocument)Document, parent, element.SourceInfo, new List<INode>())
                {
                    PreReplacementSourceInfo = element.PreReplacementSourceInfo,
                };
                closedList.Add(native, newElement);
                if (parent != null) parent.sourceChildElements.Add(newElement);
                if(native is XElement nativeElement)
                    openList.AddRange(nativeElement.Nodes().Select((node, idx) => ((XNode, XmlElement)?)(node, (XmlElement)element.ChildNodes[idx])));
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
        /// in the native HAP element.
        /// </para>
        /// </summary>
        /// <returns>The attributes collection.</returns>
        EventRaisingList<IAttribute> GetAttributesCollection()
        {
            if (!IsElement) return new EventRaisingList<IAttribute>(new List<IAttribute>());

            var sourceAttributes = ElementNode.Attributes()
                .Select(x => new XmlAttribute(x) { Element = this })
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => {
                                          var attr = ((XmlAttribute)add.Item).NativeAttribute;
                                          ElementNode.SetAttributeValue(attr.Name, attr.Value);
                                          add.Item.Element = this;
                                      },
                                      del => {
                                          ((XmlAttribute)del.Item).NativeAttribute.Remove();
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
                    var item = (XmlElement)add.Item;
                    var ele = item.NativeElement;

                    if (index >= ElementNode.Nodes().Count())
                        ElementNode.Add(ele);
                    else
                    {
                        var toInsertBefore = ElementNode.Nodes().Skip(index).First();
                        toInsertBefore.AddBeforeSelf(ele);
                    }

                    item.IsImportedNode = true;
                    item.ParentElement = this;
                },
                del => {
                    var ele = ((XmlElement)del.Item).NativeElement;
                    ele.Remove();
                    del.Item.ParentElement = null;
                });

            return eventBasedListWrapper;
        }

        IList<INode> GetSourceChildElements()
        {
            if (!IsElement) return new INode[0];

            return ElementNode.Nodes()
                .Select(x => new XmlElement(x, (XmlDocument)Doc, this, Source.CreateChild(GetLineNumber(x))))
                .Cast<INode>()
                .ToList();
        }

        /// <summary>
        /// Gets the line number for the specified element.
        /// </summary>
        /// <returns>The line number.</returns>
        /// <param name="element">The element for which to get a line number.</param>
        internal static int? GetLineNumber(XNode element)
            => ((element is IXmlLineInfo lineInfo) && lineInfo.HasLineInfo()) ? lineInfo.LineNumber : (int?) null;

        private XmlElement(XNode element,
                           XmlDocument document,
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
        /// Initializes a new instance of the <see cref="XmlElement"/> class.
        /// </summary>
        /// <param name="element">The native element object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">Source info.</param>
        public XmlElement(XNode element,
                          XmlDocument document,
                          INode parent = null,
                          ElementSourceInfo sourceInfo = null)
            : this(element, document, parent, sourceInfo, null) { }
    }
}