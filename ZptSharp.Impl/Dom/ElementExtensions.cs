using System;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Extension methods for <see cref="IElement"/>.
    /// </summary>
    public static class ElementExtensions
    {
        /// <summary>
        /// Gets the first <see cref="IAttribute"/> from the element which matches a <paramref name="spec"/>.
        /// </summary>
        /// <returns>The matching attribute, or a <see langword="null"/> reference if there is no match.</returns>
        /// <param name="element">The element from which to get the attribute.</param>
        /// <param name="spec">An attribute specification.</param>
        public static IAttribute GetMatchingAttribute(this IElement element, AttributeSpec spec)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (spec == null) throw new ArgumentNullException(nameof(spec));

            return element.Attributes.FirstOrDefault(x => x.Matches(spec));
        }

        /// <summary>
        /// Replaces the <paramref name="element"/> 'in-place' on its parent using the <paramref name="replacement"/>.
        /// Note that this means that the current element will be detached/removed from its parent as a side-effect.
        /// Further DOM manipulation should occur using the replacement element and not the replaced element.
        /// </summary>
        /// <param name="element">Element.</param>
        /// <param name="replacement">Replacement.</param>
        public static void ReplaceWith(this IElement element, IElement replacement)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
            if (element.ParentElement == null)
                throw new ArgumentException(Resources.ExceptionMessage.ElementMustHaveAParent, nameof(element));

            element.ParentElement.ReplaceChild(element, replacement);
        }
    }
}
