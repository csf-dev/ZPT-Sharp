using System;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Abstraction for a DOM document.
    /// </summary>
    public interface IDocument : IHasDocumentSourceInfo
    {
        /// <summary>
        /// Gets the root element for the current document.
        /// </summary>
        /// <returns>The root element.</returns>
        IElement GetRootElement();
    }
}
