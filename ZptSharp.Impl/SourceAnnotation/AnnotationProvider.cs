using System;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Implementation of <see cref="IGetsAnnotationForNode"/> which returns a string
    /// based on the source information for the node, along with dividing lines.
    /// </summary>
    public class AnnotationProvider : IGetsAnnotationForNode
    {
        const char dividerCharacter = '=';
        const int dividerCharCount = 78;

        readonly IGetsSourceAnnotationString sourceInfoProvider;

        /// <summary>
        /// Gets the annotation text for the node.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="node">Node.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        public string GetAnnotation(INode node, TagType tagType = TagType.Start)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return GetAnnotation(node.SourceInfo, tagType);
        }

        /// <summary>
        /// Gets the annotation text for the node, using the pre-replacement source annotation for the node.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="node">Node.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        public string GetPreReplacementAnnotation(INode node, TagType tagType = TagType.Start)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return GetAnnotation(node.PreReplacementSourceInfo, tagType);
        }

        string GetAnnotation(NodeSourceInfo sourceInfo, TagType tagType)
        {
            var divider = GetDivider();

            /* This will look roughly like:
             * 
             * ======
             * c:\foo.html (line 5)            
             * ======
             * 
             * The dividing lines above/below use dividerCharacter repeated dividerCharCount times.
             */

            return String.Concat(Environment.NewLine,
                                 divider,
                                 Environment.NewLine,
                                 GetMainAnnotation(sourceInfo, tagType),
                                 Environment.NewLine,
                                 divider,
                                 Environment.NewLine);
        }

        string GetMainAnnotation(NodeSourceInfo sourceInfo, TagType tagType)
        {
            switch(tagType)
            {
            case TagType.Start:
                return sourceInfoProvider.GetStartTagInfo(sourceInfo);
            case TagType.End:
                return sourceInfoProvider.GetEndTagInfo(sourceInfo);
            default:
                return sourceInfoProvider.GetSourceInfo(sourceInfo.Document);
            }
        }

        /// <summary>
        /// Gets a string to be used as a divider, indicating source annotation.
        /// </summary>
        public static string GetDivider() => new String(dividerCharacter, dividerCharCount);

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationProvider"/> class.
        /// </summary>
        /// <param name="sourceInfoProvider">Source info provider.</param>
        public AnnotationProvider(IGetsSourceAnnotationString sourceInfoProvider)
        {
            this.sourceInfoProvider = sourceInfoProvider ?? throw new ArgumentNullException(nameof(sourceInfoProvider));
        }
    }
}
