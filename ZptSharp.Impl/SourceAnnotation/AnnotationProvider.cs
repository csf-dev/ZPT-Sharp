using System;
using ZptSharp.Dom;
using ZptSharp.Rendering;

namespace ZptSharp.SourceAnnotation
{
    /// <summary>
    /// Implementation of <see cref="IGetsAnnotationForElement"/> which returns a string
    /// based on the source information for the element, along with dividing lines.
    /// </summary>
    public class AnnotationProvider : IGetsAnnotationForElement
    {
        const char dividerCharacter = '=';
        const int dividerCharCount = 78;

        readonly IGetsSourceAnnotationString sourceInfoProvider;

        /// <summary>
        /// Gets the annotation text for the element.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="element">Element.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        public string GetAnnotation(INode element, TagType tagType = TagType.Start)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return GetAnnotation(element.SourceInfo, tagType);
        }

        /// <summary>
        /// Gets the annotation text for the element, using the pre-replacement source annotation for the element.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="element">Element.</param>
        /// <param name="tagType">The tag type to use for the annotation.</param>
        public string GetPreReplacementAnnotation(INode element, TagType tagType = TagType.Start)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return GetAnnotation(element.PreReplacementSourceInfo, tagType);
        }

        string GetAnnotation(ElementSourceInfo sourceInfo, TagType tagType)
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

        string GetMainAnnotation(ElementSourceInfo sourceInfo, TagType tagType)
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
