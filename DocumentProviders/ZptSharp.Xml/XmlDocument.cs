using System;
using System.Xml.Linq;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IDocument"/> which is based upon
    /// a <c>System.Xml.Linq</c> <see cref="XDocument"/>.
    /// </summary>
    public class XmlDocument : DocumentBase
    {
        readonly IElement root;

        /// <summary>
        /// Gets the native HTML Agility Pack document object.
        /// </summary>
        /// <value>The native document.</value>
        public XDocument NativeDocument { get; }

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        public override IElement RootElement => root;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocument"/> class.
        /// </summary>
        /// <param name="document">The native XML document.</param>
        /// <param name="source">The source info for the document.</param>
        public XmlDocument(XDocument document, IDocumentSourceInfo source) : base(source)
        {
            NativeDocument = document ?? throw new ArgumentNullException(nameof(document));

            var nativeRoot = NativeDocument.Root;
            var src = new ElementSourceInfo(Source, XmlElement.GetLineNumber(nativeRoot));
            root = new XmlElement(nativeRoot, this, sourceInfo: src);
        }
    }
}
