using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public class XmlElement : ElementBase
    {
        public XElement NativeElement { get; }

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
        /// <see cref="XmlElement"/>.  This shows the element's start-tag.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="XmlElement"/>.</returns>
        public override string ToString()
        {
            var attributes = NativeElement.Attributes()
                .Select(attrib => $"{attrib.Name}=\"{attrib.Value}\"")
                .ToList();
            var hasAttributes = attributes.Count > 0;

            return $"<{NativeElement.Name}{(hasAttributes ? " " : String.Empty)}{String.Join(" ", attributes)}>";
        }

        static int? GetLineNumber(XElement element)
            => ((element is IXmlLineInfo lineInfo) && lineInfo.HasLineInfo()) ? lineInfo.LineNumber : (int?) null;

        public XmlElement(XElement element,
                          XmlDocument document,
                          IElementSourceInfo sourceInfo = null) : base(document, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}