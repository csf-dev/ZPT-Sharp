using System;
using AngleSharp.Html.Dom;

namespace ZptSharp.Dom
{
    public class AngleSharpDocument : DocumentBase
    {
        public IHtmlDocument NativeDocument { get; }

        public override IElement GetRootElement()
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
