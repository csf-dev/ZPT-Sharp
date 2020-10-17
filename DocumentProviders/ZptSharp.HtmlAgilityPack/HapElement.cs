using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class HapElement : IElement
    {
        public HtmlNode NativeElement { get; }

        public IDocument Document { get; }

        public IElementSourceInfo SourceInfo { get; }

        IDocumentSourceInfo IHasDocumentSourceInfo.SourceInfo => SourceInfo.Document;

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IList<IAttribute> Attributes { get { throw new NotImplementedException(); } }

        public HapElement(HtmlNode element,
                          HapDocument document,
                          IElementSourceInfo sourceInfo = null)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
            Document = document ?? throw new ArgumentNullException(nameof(document));
            SourceInfo = sourceInfo ?? new ElementSourceInfo(document.SourceInfo, NativeElement.Line);
        }
    }
}
