using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class HapElement : ElementBase
    {
        public HtmlNode NativeElement { get; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public override IList<IAttribute> Attributes { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        public override IList<IElement> ChildElements { get { throw new NotImplementedException(); } }

        /// <summary>
        /// Returns a <see cref="String"/> that represents the current
        /// <see cref="HapElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="HapElement"/>.</returns>
        public override string ToString()
        {
            var attributes = NativeElement.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attributes.Count > 0;

            return $"<{NativeElement.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attributes)}>";
        }

        public HapElement(HtmlNode element,
                          HapDocument document,
                          IElementSourceInfo sourceInfo = null) : base(document, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
