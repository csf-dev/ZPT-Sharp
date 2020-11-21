using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="INode"/> which is based upon an XML <see cref="XElement"/>.
    /// </summary>
    public class XmlElement : ElementBase
    {
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
        public override IList<IAttribute> Attributes { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public override IList<INode> ChildNodes { get { throw new NotImplementedException(); } }

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

            var attributes = ElementNode.Attributes()
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attributes.Count > 0;

            return $"<{ElementNode.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attributes)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override INode GetCopy()
        {
            throw new NotImplementedException();

            //var copiedElement = new XElement(NativeElement);
            //return new XmlElement(copiedElement, (XmlDocument) Document, null, SourceInfo);
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
        /// Gets the line number for the specified element.
        /// </summary>
        /// <returns>The line number.</returns>
        /// <param name="element">The element for which to get a line number.</param>
        internal static int? GetLineNumber(XElement element)
            => ((element is IXmlLineInfo lineInfo) && lineInfo.HasLineInfo()) ? lineInfo.LineNumber : (int?) null;

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
                          ElementSourceInfo sourceInfo = null) : base(document, parent, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}