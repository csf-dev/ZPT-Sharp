using System;
using ZptSharp.Dom;

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
        /// <param name="useStartTag"><c>true</c> to use the line-number for the element's start-tag; <c>false</c> to use the end tag</param>
        string GetAnnotation(INode element, bool useStartTag = true);
    }
}
