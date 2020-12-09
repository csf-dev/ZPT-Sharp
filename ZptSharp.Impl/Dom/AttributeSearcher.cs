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

        /// <summary>
        /// Recursively gets every <see cref="INode"/> contained within the
        /// <paramref name="source"/>.  This includes the source object if it
        /// itself is also an element.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method uses an open/closed list to search through &amp; return results.
        /// The open list is a list/queue of the elements which are still to be searched.
        /// The closed list is the list of results which have been found.
        /// </para>
        /// </remarks>
        /// <returns>The collection of elements.</returns>
        /// <param name="source">Source.</param>
        IEnumerable<INode> RecursivelyGetAllElements(IHasElements source)
        {
            INode current;
            var closedList = new List<INode>();

            if (source is INode element) closedList.Add(element);

            for (var openList = new List<INode>(source.GetChildElements());
                 (current = openList.FirstOrDefault()) != null;
                 openList.Remove(current))
            {
                closedList.Add(current);
                openList.AddRange(current.ChildNodes);
            }

            return closedList;
        }
    }
}
