using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    public static class HapDocumentUtil
    {
        static readonly HapDocumentProvider provider = new HapDocumentProvider();

        /// <summary>
        /// Gets a single HTML node node from the first node in the specified HTML string.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="html">Html.</param>
        public static HapNode GetNode(string html)
            => (HapNode)GetDocument(html).RootNode.ChildNodes.First();

        /// <summary>
        /// Gets a document from the specified HTML string.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="html">Html.</param>
        public static HapDocument GetDocument(string html)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(html)))
                return (HapDocument)provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
        }

        /// <summary>
        /// Gets a rendering of a document.
        /// </summary>
        /// <returns>The rendering.</returns>
        /// <param name="doc">Document.</param>
        public static async Task<string> GetRendering(HapDocument doc)
        {
            using (var stream = await provider.WriteDocumentAsync(doc, RenderingConfig.Default))
                return await Util.TestFiles.GetString(stream);
        }
    }
}
