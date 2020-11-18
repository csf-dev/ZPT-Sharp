using System;
using ZptSharp.Dom;

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

        /// <summary>
        /// Gets the annotation text for the element.
        /// </summary>
        /// <returns>The annotation text.</returns>
        /// <param name="element">Element.</param>
        /// <param name="useStartTag"><c>true</c> to use the line-number for the element's start-tag; <c>false</c> to use the end tag</param>
        public string GetAnnotation(INode element, bool useStartTag = true)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var divider = new String(dividerCharacter, dividerCharCount);

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
                                 GetMainAnnotation(element, useStartTag),
                                 Environment.NewLine,
                                 divider,
                                 Environment.NewLine);
        }

        string GetMainAnnotation(INode element, bool useStartTag)
        {
            var source = element.SourceInfo;
            var docSource = source.Document.ToString();

            var lineNumber = useStartTag
                ? source.StartTagLineNumber
                : source.EndTagLineNumber;

            return lineNumber.HasValue
                ? $"{docSource} (line {lineNumber})"
                : docSource;
        }
    }
}
