using System;
using System.Xml.Linq;

namespace ZptSharp.Dom
{
    public class XmlDocument : IDocument
    {
        public XDocument Document { get; }

        public XmlDocument(XDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
