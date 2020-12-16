using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    public static class AngleSharpDocumentUtil
    {
        const string emptyHtml = @"<html>
<head>
<title></title>
</head>
<body>
</body>
</html>";

        static readonly AngleSharpDocumentProvider provider = new AngleSharpDocumentProvider();

        /// <summary>
        /// Gets a single HTML element node from the first element in the specified HTML string.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="html">Html.</param>
        public static AngleSharpElement GetNode(string html)
        {
            var document = GetDocument(emptyHtml);
            var context = BrowsingContext.New();
            var parser = context.GetService<IHtmlParser>();
            var bodyElement = document.RootElement.ChildNodes[1];
            var nativeBody = ((AngleSharpElement)bodyElement).NativeElement as IElement;
            var nodes = parser.ParseFragment(html, nativeBody);
            return new AngleSharpElement(nodes.First(), document);
        }

        /// <summary>
        /// Gets a document from the specified HTML string.
        /// </summary>
        /// <returns>The document.</returns>
        /// <param name="html">Html.</param>
        public static AngleSharpDocument GetDocument(string html)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(html)))
                return (AngleSharpDocument) provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
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
