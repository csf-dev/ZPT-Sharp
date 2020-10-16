using System;
using AngleSharp.Html.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class AngleSharpDocument : IDocument
    {
        public IHtmlDocument NativeDocument { get; }

        public IDocumentSourceInfo SourceInfo { get; set; }

        public IElement GetRootElement()
        {
            var native = NativeDocument.DocumentElement;
            return new AngleSharpElement(native, this);
        }

        public AngleSharpDocument(IHtmlDocument document)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));
        }
    }
}
