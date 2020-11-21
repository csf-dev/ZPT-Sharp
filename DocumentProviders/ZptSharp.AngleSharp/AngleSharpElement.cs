using System;
using System.Collections.Generic;
using System.Linq;
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

            return $"<{ElementNode.TagName}{(hasAttributes? " " : String.Empty)}{String.Join(" ", attrs)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override INode GetCopy()
        {
            var copiedElement = ElementNode.Clone();
            return new AngleSharpElement(copiedElement, (AngleSharpDocument) Document, null, SourceInfo);
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

            return String.Equals(ElementNode.Prefix, @namespace.Prefix, StringComparison.InvariantCulture);
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
            if(!IsElement) return new EventRaisingList<IAttribute>(new List<IAttribute>());

            var sourceAttributes = ElementNode.Attributes
                .Select(x => new AngleSharpAttribute(x, this))
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => ElementNode.Attributes.SetNamedItem(((AngleSharpAttribute)add.Item).NativeAttribute),
                                      del => ElementNode.Attributes.RemoveNamedItem(del.Item.Name));

            return attribs;
        }

        /// <summary>
        /// <para>
        /// Called by the constructor; initialises and returns a <see cref="EventRaisingList{IElement}"/>
        /// for use as the <see cref="ChildNodes"/> collection.
        /// </para>
        /// <para>
        /// This event-raising list is used to keep the child elements collection in-sync with the child
        /// elements in the native AngleSharp element.
        /// </para>
        /// </summary>
        /// <returns>The child elements collection.</returns>
        EventRaisingList<INode> GetChildElementsCollection()
        {
            if(!IsElement) return new EventRaisingList<INode>(new List<INode>());

            var sourceChildElements = ElementNode.Children
                .Select(x => new AngleSharpElement(x, (AngleSharpDocument) Doc, this, Source.CreateChild(x.SourceReference?.Position.Line)))
                .Cast<INode>()
                .ToList();

            var children = new EventRaisingList<INode>(sourceChildElements);

            children.SetupAfterActions(
                add => {
                    var index = ((IList<INode>)add.Collection).IndexOf(add.Item);
                    var item = (AngleSharpElement)add.Item;
                    var ele = item.NativeElement;

                    if (index >= ElementNode.ChildElementCount)
                        ElementNode.Append(ele);
                    else
                        ElementNode.InsertBefore(ele, ElementNode.Children[index]);

                    item.IsImportedNode = true;
                    item.ParentElement = this;
                },
                del => {
                    var ele = ((AngleSharpElement)del.Item).ElementNode;
                    ElementNode.RemoveChild(ele);
                    del.Item.ParentElement = null;
                });

            return children;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleSharpElement"/> class.
        /// </summary>
        /// <param name="nativeElement">The AngleSharp native element object.</param>
        /// <param name="document">The AngleSharp document object.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">The source info for the element.</param>
        public AngleSharpElement(As.INode nativeElement,
                                 AngleSharpDocument document,
                                 INode parent = null,
                                 ElementSourceInfo sourceInfo = null) : base(document, parent, sourceInfo)
        {
            NativeElement = nativeElement ?? throw new ArgumentNullException(nameof(nativeElement));
            attributes = GetAttributesCollection();
            childElements = GetChildElementsCollection();
        }
    }
}
