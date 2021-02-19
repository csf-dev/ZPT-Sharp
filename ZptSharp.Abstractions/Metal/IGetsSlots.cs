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
        /// Gets the defined slots from the specified <paramref name="node"/> and its descendents.
        /// </summary>
        /// <returns>The defined slots.</returns>
        /// <param name="node">Node.</param>
        IEnumerable<Slot> GetDefinedSlots(INode node);

        /// <summary>
        /// Gets the slot fillers from the specified <paramref name="node"/> and its descendents.
        /// </summary>
        /// <returns>The slot fillers.</returns>
        /// <param name="node">Node.</param>
        IEnumerable<Slot> GetSlotFillers(INode node);
    }
}
