using System;
using HtmlAgilityPack;

namespace ZptSharp.Dom
{
    public class HapDocument : IDocument
    {
        public HtmlDocument Document { get; }

        public HapDocument(HtmlDocument document)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
