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
        readonly EventRaisingList<IAttribute> attributes;
        readonly EventRaisingList<INode> childElements;

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
            return new HapElement(copiedElement, (HapDocument) Document, null, SourceInfo);
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

            return String.Equals(nameParts[1], @namespace.Prefix, StringComparison.InvariantCulture);
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

            attribs.SetupAfterActions(add => NativeElement.Attributes[add.Item.Name] = ((HapAttribute) add.Item).NativeAttribute,
                                      del => NativeElement.Attributes.Remove(del.Item.Name));

            return attribs;
        }

        /// <summary>
        /// <para>
        /// Called by the constructor; initialises and returns a <see cref="EventRaisingList{IElement}"/>
        /// for use as the <see cref="ChildNodes"/> collection.
        /// </para>
        /// <para>
        /// This event-raising list is used to keep the child elements collection in-sync with the child
        /// elements in the native HAP element.
        /// </para>
        /// </summary>
        /// <returns>The child elements collection.</returns>
        EventRaisingList<INode> GetChildElementsCollection()
        {
            var sourceChildElements = NativeElement.ChildNodes
                .Select(x => new HapElement(x, (HapDocument)Doc, this, Source.CreateChild(x.Line)))
                .Cast<INode>()
                .ToList();

            var children = new EventRaisingList<INode>(sourceChildElements);

            children.SetupAfterActions(
                add => {
                    var index = ((IList<INode>)add.Collection).IndexOf(add.Item);
                    var ele = ((HapElement)add.Item).NativeElement;

                    if (index >= NativeElement.ChildNodes.Count)
                        NativeElement.AppendChild(ele);
                    else
                        NativeElement.InsertBefore(ele, NativeElement.ChildNodes[index]);

                    add.Item.ParentElement = this;
                },
                del => {
                    var ele = ((HapElement)del.Item).NativeElement;
                    NativeElement.RemoveChild(ele);
                    del.Item.ParentElement = null;
                });

            return children;
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
                          IElementSourceInfo sourceInfo = null) : base(document, parent, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
            attributes = GetAttributesCollection();
            childElements = GetChildElementsCollection();
        }
    }
}
