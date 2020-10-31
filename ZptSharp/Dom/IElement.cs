﻿using System;
using System.Collections.Generic;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM element.
    /// </summary>
    public interface IElement : IHasDocumentSourceInfo, IHasElements
    {
        /// <summary>
        /// Gets the parent document for the current element.
        /// </summary>
        /// <value>The document.</value>
        IDocument Document { get; }

        /// <summary>
        /// Gets information which indicates the original source of the element (for example, a file path and line number).
        /// </summary>
        /// <value>The source info.</value>
        new IElementSourceInfo SourceInfo { get; }

        /// <summary>
        /// Gets a collection of the element's attributes.
        /// </summary>
        /// <value>The attributes.</value>
        IList<IAttribute> Attributes { get; }

        /// <summary>
        /// Gets the elements contained within the current element.
        /// </summary>
        /// <value>The child elements.</value>
        IList<IElement> ChildElements { get; }
    }
}
