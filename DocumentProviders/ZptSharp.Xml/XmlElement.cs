using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class XmlElement : IElement
    {
        public XElement NativeElement { get; }

        public IDocument Document { get; }

        public IElementSourceInfo SourceInfo { get; }

        IDocumentSourceInfo IHasDocumentSourceInfo.SourceInfo => SourceInfo.Document;

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IList<IAttribute> Attributes { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public IList<IElement> ChildElements { get { throw new NotImplementedException(); } }

        static int? GetLineNumber(XElement element)
        {
            IXmlLineInfo lineInfo = element;
            if (!lineInfo.HasLineInfo()) return null;
            return lineInfo.LineNumber;
        }

        public XmlElement(XElement element,
                          XmlDocument document,
                          IElementSourceInfo sourceInfo = null)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
            Document = document ?? throw new ArgumentNullException(nameof(document));
            SourceInfo = sourceInfo ?? new ElementSourceInfo(document.SourceInfo, GetLineNumber(element));
        }
    }
}