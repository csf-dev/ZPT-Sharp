using System;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which may provide information about the source of a document (for example a file on disk).
    /// </summary>
    public interface IHasDocumentSourceInfo
    {
        /// <summary>
        /// Gets information which indicates the original source of the document (for example, a file path).
        /// </summary>
        /// <value>The source info.</value>
        IDocumentSourceInfo SourceInfo { get; }
    }
}
