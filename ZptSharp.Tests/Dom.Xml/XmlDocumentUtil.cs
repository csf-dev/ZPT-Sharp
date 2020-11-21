using System;
using System.IO;
using System.Linq;
using System.Text;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    public static class XmlDocumentUtil
    {
        /// <summary>
        /// Gets a single XML element node from the first element in the specified XML string.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="xml">Xml.</param>
        public static XmlElement GetNode(string xml)
            => (XmlElement)GetDocument(xml).RootElement;

        /// <summary>
        /// Gets a document from the specified HTML string.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="xml">XML.</param>
        public static XmlDocument GetDocument(string xml)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var provider = new XmlDocumentProvider();
                return (XmlDocument)provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
            }
        }
    }
}
