using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Collections.EventRaising;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="INode"/> which is based upon an AngleSharp <see cref="AngleSharp.Dom.IElement"/>.
    /// </summary>
    public class AngleSharpElement : ElementBase
    {
        readonly EventRaisingList<IAttribute> attributes;
        readonly EventRaisingList<INode> childElements;

        /// <summary>
        /// Gets the native AngleSharp <see cref="AngleSharp.Dom.IElement"/> instance which
        /// acts as the basis for the current element.
        /// </summary>
        /// <value>The native AngleSharp element object.</value>
        public AngleSharp.Dom.IElement NativeElement { get; }

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
        public override bool IsElement => NativeElement.NodeType == AngleSharp.Dom.NodeType.Element;

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="AngleSharpElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="AngleSharpElement"/>.</returns>
        public override string ToString()
        {
            var attrs = NativeElement.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attrs.Count > 0;

            return $"<{NativeElement.TagName}{(hasAttributes? " " : String.Empty)}{String.Join(" ", attrs)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override INode GetCopy()
        {
            var copiedElement = (AngleSharp.Dom.IElement) NativeElement.Clone();
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

            return String.Equals(NativeElement.Prefix, @namespace.Prefix, StringComparison.InvariantCulture);
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
            var sourceAttributes = NativeElement.Attributes
                .Select(x => new AngleSharpAttribute(x, this))
                .Cast<IAttribute>()
                .ToList();
            var attribs = new EventRaisingList<IAttribute>(sourceAttributes);

            attribs.SetupAfterActions(add => NativeElement.Attributes.SetNamedItem(((AngleSharpAttribute)add.Item).NativeAttribute),
                                      del => NativeElement.Attributes.RemoveNamedItem(del.Item.Name));

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
            var sourceChildElements = NativeElement.Children
                .Select(x => new AngleSharpElement(x, (AngleSharpDocument) Doc, this, Source.CreateChild(x.SourceReference?.Position.Line)))
                .Cast<INode>()
                .ToList();

            var children = new EventRaisingList<INode>(sourceChildElements);

            children.SetupAfterActions(
                add => {
                    var index = ((IList<INode>)add.Collection).IndexOf(add.Item);
                    var ele = ((AngleSharpElement)add.Item).NativeElement;

                    if (index >= NativeElement.ChildElementCount)
                        NativeElement.Append(ele);
                    else
                        NativeElement.InsertBefore(ele, NativeElement.Children[index]);

                    add.Item.ParentElement = this;
                },
                del => {
                    var ele = ((AngleSharpElement)del.Item).NativeElement;
                    NativeElement.RemoveChild(ele);
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
        public AngleSharpElement(AngleSharp.Dom.IElement nativeElement,
                                 AngleSharpDocument document,
                                 INode parent = null,
                                 IElementSourceInfo sourceInfo = null) : base(document, parent, sourceInfo)
        {
            NativeElement = nativeElement ?? throw new ArgumentNullException(nameof(nativeElement));
            attributes = GetAttributesCollection();
            childElements = GetChildElementsCollection();
        }
    }
}
