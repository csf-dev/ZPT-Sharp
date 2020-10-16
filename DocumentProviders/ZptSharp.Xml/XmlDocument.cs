using System;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class XmlDocument : IDocument
    {
        public XDocument NativeDocument { get; }

        public IDocumentSourceInfo SourceInfo { get; set; }

        public IElement GetRootElement()
        {
            var native = NativeDocument.Root;
            return new XmlElement(native, this);
        }

        public XmlDocument(XDocument document)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
