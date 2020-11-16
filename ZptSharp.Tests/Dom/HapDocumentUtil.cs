using System;
using System.IO;
using System.Linq;
using System.Text;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    public static class HapDocumentUtil
    {
        /// <summary>
        /// Gets a single HTML element node from the first element in the specified HTML string.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="html">Html.</param>
        public static HapElement GetNode(string html)
            => (HapElement)GetDocument(html).RootElement.ChildNodes.First();

        /// <summary>
        /// Gets a document from the specified HTML string.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="html">Html.</param>
        public static HapDocument GetDocument(string html)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(html)))
            {
                var provider = new HapDocumentProvider();
                return (HapDocument)provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
            }
        }
    }
}
