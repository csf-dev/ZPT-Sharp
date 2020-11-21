using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// An object which gets the annotation text for a specified element.
    /// </summary>
    public interface IGetsAnnotationForElement
    {
        /// <summary>
        /// Gets the annotation text for the element.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="element">Element.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        string GetAnnotation(INode element, TagType tagType = TagType.Start);

        /// <summary>
        /// Gets the annotation text for the element, using the pre-replacement source annotation for the element.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="element">Element.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        string GetPreReplacementAnnotation(INode element, TagType tagType = TagType.Start);
    }
}
