using System;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    public interface IHasDocumentSourceInfo
    {
        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        IDocumentSourceInfo SourceInfo { get; }
    }
}
