﻿using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstract base class for an <see cref="IDocument"/>, containing functionality
    /// which is neutral to the specific implementation.
    /// </summary>
    public abstract class DocumentBase : IDocument
    {
        /// <summary>
        /// A field to get the document source info.
        /// </summary>
        protected readonly IDocumentSourceInfo Source;

        IEnumerable<IElement> IHasElements.GetChildElements() => new[] { RootElement };

        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        public virtual IDocumentSourceInfo SourceInfo => Source;

        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        public abstract IElement RootElement { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBase"/> class.
        /// </summary>
        /// <param name="source">The source info for the document.</param>
        public DocumentBase(IDocumentSourceInfo source)
        {
            Source = source ?? throw new System.ArgumentNullException(nameof(source));
        }
    }
}
