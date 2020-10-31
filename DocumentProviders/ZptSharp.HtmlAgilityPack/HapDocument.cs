using System;
using HtmlAgilityPack;

namespace ZptSharp.Dom
{
    public class HapDocument : DocumentBase
    {
        public HtmlDocument NativeDocument { get; }

        public override IElement GetRootElement()
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
