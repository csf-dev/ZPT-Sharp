using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An object which gets the annotation text for a specified node.
    /// </summary>
    public interface IGetsAnnotationForNode
    {
        /// <summary>
        /// Gets the annotation text for the node.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="node">Node.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        string GetAnnotation(INode node, TagType tagType = TagType.Start);

        /// <summary>
        /// Gets the annotation text for the node, using the pre-replacement source annotation for the node.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="node">Node.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        string GetPreReplacementAnnotation(INode node, TagType tagType = TagType.Start);
    }
}
