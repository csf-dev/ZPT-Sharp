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
    }
}
