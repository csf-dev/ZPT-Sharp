using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    public static class XmlDocumentUtil
    {
        static readonly XmlDocumentProvider
            provider = new XmlDocumentProvider(new XmlReaderSettingsFactory(new Resources.EmbeddedResourceXhtmlDtdProvider(),
                                               Mock.Of<IServiceProvider>(x => x.GetService(typeof(RenderingConfig)) == RenderingConfig.Default)));

        /// <summary>
        /// Gets a single XML node node from the first node in the specified XML string.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="xml">Xml.</param>
        public static XmlNode GetNode(string xml)
            => (XmlNode)GetDocument(xml).RootNode;

        /// <summary>
        /// Gets a document from the specified HTML string.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="xml">XML.</param>
        public static XmlDocument GetDocument(string xml)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                return (XmlDocument)provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
        }

        /// <summary>
        /// Gets a rendering of a document.
        /// </summary>
        /// <returns>The rendering.</returns>
        /// <param name="doc">Document.</param>
        public static async Task<string> GetRendering(XmlDocument doc)
        {
            using (var stream = await provider.WriteDocumentAsync(doc, RenderingConfig.Default))
                return await Util.TestFiles.GetString(stream);
        }
    }
}
