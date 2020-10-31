using System;
using System.Xml.Linq;

namespace ZptSharp.Dom
{
    public class XmlDocument : DocumentBase
    {
        public XDocument NativeDocument { get; }

        public override IElement GetRootElement()
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
