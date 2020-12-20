using System;
using System.Collections.Generic;
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
        const string emptyHtml = @"<html><head></head><body></body></html>";

        static readonly AngleSharpDocumentProvider provider = new AngleSharpDocumentProvider();

        /// <summary>
        /// Gets a single HTML node node from the first node in the specified HTML string.
        /// This is specifically for use when the <paramref name="html"/> is a fragment and
        /// not a complete document, containing the basic HTML 'boilerplate' of <c>&lt;html&gt;</c>,
        /// <c>&lt;head&gt;</c> &amp; <c>&lt;body&gt;</c> tags.
        /// </summary>
        /// <returns>The node.</returns>
        /// <param name="html">Html.</param>
        public static AngleSharpNode GetNodeFromFragment(string html)
        {
            var document = GetDocument(emptyHtml);
            var context = BrowsingContext.New();
            var parser = context.GetService<IHtmlParser>();
            var bodyNode = GetBodyNode(document);
            var nativeBody = ((AngleSharpNode)bodyNode).NativeNode as INode;
            var nodes = parser.ParseFragment(html, nativeBody);
            return new AngleSharpNode(nodes.First(), document);
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
        /// Gets a collection of the nodes from a parsed document.  This is for use when
        /// the original/source document was created from a fragment which did not include
        /// the basic HTML 'boilerplate' of <c>&lt;html&gt;</c>, <c>&lt;head&gt;</c> &amp;
        /// <c>&lt;body&gt;</c> tags.
        /// </summary>
        /// <returns>The nodes from the document's body node.</returns>
        /// <param name="doc">Document.</param>
        public static IEnumerable<AngleSharpNode> GetNodesFromFragmentBasedDocument(IDocument doc)
            => GetBodyNode(doc).ChildNodes.Cast<AngleSharpNode>().ToList();

        static AngleSharpNode GetBodyNode(IDocument doc) => doc.RootNode.ChildNodes[1] as AngleSharpNode;

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
