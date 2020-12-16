using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    public static class AngleSharpDocumentUtil
    {
        static readonly AngleSharpDocumentProvider provider = new AngleSharpDocumentProvider();

        /// <summary>
        /// Gets a single HTML element node from the first element in the specified HTML string.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="html">Html.</param>
        public static AngleSharpElement GetNode(string html)
            => (AngleSharpElement)GetDocument(html).RootElement.ChildNodes.First();

        /// <summary>
        /// Gets a document from the specified HTML string.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="html">Html.</param>
        public static AngleSharpDocument GetDocument(string html)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(html)))
                return (AngleSharpDocument)provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
        }

        /// <summary>
        /// Gets a rendering of a document.
        /// </summary>
        /// <returns>The rendering.</returns>
        /// <param name="doc">Document.</param>
        public static async Task<string> GetRendering(AngleSharpDocument doc)
        {
            using (var stream = await provider.WriteDocumentAsync(doc, RenderingConfig.Default))
                return await Util.TestFiles.GetString(stream);
        }
    }
}
