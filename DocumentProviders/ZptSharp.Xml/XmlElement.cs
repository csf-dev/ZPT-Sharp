using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IElement"/> which is based upon an XML <see cref="XElement"/>.
    /// </summary>
    public class XmlElement : ElementBase
    {
        /// <summary>
        /// Gets the native XML <see cref="XElement"/> instance which
        /// acts as the basis for the current element.
        /// </summary>
        /// <value>The native XML element object.</value>
        public XElement NativeElement { get; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public override IList<IAttribute> Attributes { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public override IList<IElement> ChildElements { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="XmlElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="XmlElement"/>.</returns>
        public override string ToString()
        {
            var attributes = NativeElement.Attributes()
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attributes.Count > 0;

            return $"<{NativeElement.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attributes)}>";
        }

        /// <summary>
        /// Gets a copy of the current element and all of its children.
        /// </summary>
        /// <returns>The copied element.</returns>
        public override IElement GetCopy()
        {
            var copiedElement = new XElement(NativeElement);
            return new XmlElement(copiedElement, (XmlDocument) Document, ParentElement, SourceInfo);
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

            return String.Equals(NativeElement.Name.NamespaceName, @namespace.Uri, StringComparison.InvariantCulture);
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
        public XmlElement(XElement element,
                          XmlDocument document,
                          IElement parent = null,
                          IElementSourceInfo sourceInfo = null) : base(document, parent, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}