using System;
using System.Collections.Generic;
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

        static int? GetLineNumber(XElement element)
        {
            IXmlLineInfo lineInfo = element;
            if (!lineInfo.HasLineInfo()) return null;
            return lineInfo.LineNumber;
        }

        public XmlElement(XElement element,
                          XmlDocument document,
                          IElementSourceInfo sourceInfo = null) : base(document, sourceInfo)
        {
            NativeElement = element ?? throw new ArgumentNullException(nameof(element));
        }
    }
}