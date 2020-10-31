using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="IDocument"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class DocumentBase : IDocument
    {
        IEnumerable<IElement> IHasElements.GetChildElements() => new[] { GetRootElement() };

        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        public virtual IDocumentSourceInfo SourceInfo { get; set; }

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        public abstract IElement GetRootElement();
    }
}
