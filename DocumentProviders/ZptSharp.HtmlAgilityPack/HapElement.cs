using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IElement"/> which is based upon an HTML Agility Pack <see cref="HtmlNode"/>.
    /// </summary>
    public class HapElement : ElementBase
    {
        /// <summary>
        /// Gets the native HTML Agility Pack <see cref="HtmlNode"/> instance which
        /// acts as the basis for the current element.
        /// </summary>
        /// <value>The native HTML Agility Pack element object.</value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="HapElement"/> class.
        /// </summary>
        /// <param name="element">The native element object.</param>
        /// <param name="document">The containing document.</param>
        /// <param name="parent">The parent element.</param>
        /// <param name="sourceInfo">Source info.</param>
        public HapElement(HtmlNode element,
                          HapDocument document,
                          IElement parent = null,
                          IElementSourceInfo sourceInfo = null) : base(document, parent, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
