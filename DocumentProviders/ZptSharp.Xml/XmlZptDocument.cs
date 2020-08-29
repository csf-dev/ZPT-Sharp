using System;
using System.Xml.Linq;

namespace ZptSharp.Dom
{
    public class XmlZptDocument : IDocument
    {
        public XDocument Document { get; }

        public XmlZptDocument(XDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
