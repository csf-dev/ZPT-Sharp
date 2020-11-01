using System;
using System.Collections.Generic;
using System.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class AngleSharpElement : ElementBase
    {
        public AngleSharp.Dom.IElement NativeElement { get; }

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
        /// <see cref="AngleSharpElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="AngleSharpElement"/>.</returns>
        public override string ToString()
        {
            var attributes = NativeElement.Attributes
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attributes.Count > 0;

            return $"<{NativeElement.TagName}{(hasAttributes? " " : String.Empty)}{String.Join(" ", attributes)}>";
        }

        public AngleSharpElement(AngleSharp.Dom.IElement element,
                                 AngleSharpDocument document,
                                 IElementSourceInfo sourceInfo = null) : base(document, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}
