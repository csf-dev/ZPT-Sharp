using System.Collections.Generic;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which gets a collection of <see cref="Slot"/> instances from a
    /// specified <see cref="INode"/> and its descendents.
    /// </summary>
    public interface IGetsSlots
    {
        /// <summary>
        /// Gets the defined slots from the specified <paramref name="element"/> and its descendents.
        /// </summary>
        /// <returns>The defined slots.</returns>
        /// <param name="element">Element.</param>
        IEnumerable<Slot> GetDefinedSlots(INode element);

        /// <summary>
        /// Gets the slot fillers from the specified <paramref name="element"/> and its descendents.
        /// </summary>
        /// <returns>The slot fillers.</returns>
        /// <param name="element">Element.</param>
        IEnumerable<Slot> GetSlotFillers(INode element);
    }
}
