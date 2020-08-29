using System;
using AngleSharp.Html.Dom;

namespace ZptSharp.Dom
{
    public class AngleSharpDocument : IDocument
    {
        public IHtmlDocument Document { get; }

        public AngleSharpDocument(IHtmlDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
