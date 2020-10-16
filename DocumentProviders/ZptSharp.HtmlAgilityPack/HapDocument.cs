using System;
using HtmlAgilityPack;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class HapDocument : IDocument
    {
        public HtmlDocument NativeDocument { get; }

        public IDocumentSourceInfo SourceInfo { get; set; }

        public IElement GetRootElement()
        {
            var native = NativeDocument.DocumentNode;
            return new HapElement(native, this);
        }

        public HapDocument(HtmlDocument document)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
