using System;
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