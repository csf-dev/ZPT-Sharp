using System.Collections.Generic;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which may perform a recursive search of a specified container, finding all
    /// attributes which match a spec.
    /// </summary>
    public interface ISearchesForAttributes
    {
        /// <summary>
        /// Gets all attributes within the <paramref name="source"/> which match a spec.
        /// This performs a recursive search.
        /// </summary>
        /// <returns>The matching attributes.</returns>
        /// <param name="source">A source object which provides nodes.</param>
        /// <param name="spec">A spec for an attribute.</param>
        IEnumerable<IAttribute> SearchForAttributes(IHasNodes source, AttributeSpec spec);
    }
}
