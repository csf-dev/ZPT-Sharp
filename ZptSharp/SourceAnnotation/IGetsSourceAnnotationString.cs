using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An object which gets a string representation of source information, for source annotation.
    /// </summary>
    public interface IGetsSourceAnnotationString
    {
        /// <summary>
        /// Gets a string which represents information about a document's source information.
        /// </summary>
        /// <returns>The source info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        string GetSourceInfo(IDocumentSourceInfo sourceInfo);

        /// <summary>
        /// Gets a string which represents information about the source of an element start-tag.
        /// </summary>
        /// <returns>The start-tag info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        string GetStartTagInfo(ElementSourceInfo sourceInfo);

        /// <summary>
        /// Gets a string which represents information about the source of an element end-tag.
        /// </summary>
        /// <returns>The end-tag info.</returns>
        /// <param name="sourceInfo">The source info from which to get a string.</param>
        string GetEndTagInfo(ElementSourceInfo sourceInfo);
    }
}
