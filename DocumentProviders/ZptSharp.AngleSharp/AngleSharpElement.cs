using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class AngleSharpElement : IElement
    {
        public AngleSharp.Dom.IElement NativeElement { get; }

        public IDocument Document { get; }

        public IElementSourceInfo SourceInfo { get; }

        IDocumentSourceInfo IHasDocumentSourceInfo.SourceInfo => SourceInfo.Document;

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public IList<IAttribute> Attributes { get { throw new NotImplementedException(); } }

        public AngleSharpElement(AngleSharp.Dom.IElement element,
                                 AngleSharpDocument document,
                                 IElementSourceInfo sourceInfo = null)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
            Document = document ?? throw new ArgumentNullException(nameof(document));
            SourceInfo = sourceInfo ?? new ElementSourceInfo(document.SourceInfo, NativeElement.SourceReference?.Position.Line);
        }
    }
}
