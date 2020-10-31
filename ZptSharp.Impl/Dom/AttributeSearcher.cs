using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Performs a recursive search for attributes which match a specification.
    /// </summary>
    public class AttributeSearcher : ISearchesForAttributes
    {
        /// <summary>
        /// Gets all attributes within the <paramref name="source"/> which match a spec.
        /// This performs a recursive search.
        /// </summary>
        /// <returns>The matching attributes.</returns>
        /// <param name="source">A source object which provides elements.</param>
        /// <param name="spec">A spec for an attribute.</param>
        public IEnumerable<IAttribute> SearchForAttributes(IHasElements source, AttributeSpec spec)
        {
            if (source == null)
                throw new System.ArgumentNullException(nameof(source));
            if (spec == null)
                throw new System.ArgumentNullException(nameof(spec));

            var elements = RecursivelyGetAllElements(source);

            return elements
                .SelectMany(x => x.Attributes)
                .Where(a => a.Matches(spec))
                .ToList();
        }

        IEnumerable<IElement> RecursivelyGetAllElements(IHasElements source)
        {
            List<IElement>
                openList = new List<IElement>(source.GetChildElements()),
                closedList = new List<IElement>();

            if (source is IElement element) closedList.Add(element);

            while(openList.Any())
            {
                var current = openList.First();
                closedList.Add(current);
                openList.AddRange(current.ChildElements);
                openList.Remove(current);
            }

            return closedList;
        }
    }
}
