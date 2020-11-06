using System;
using System.Collections.Generic;

namespace ZptSharp.Metal
{
    /// <summary>
    /// An object which fills slots in METAL macros.
    /// </summary>
    public interface IFillsSlots
    {
        /// <summary>
        /// Fills any of the <paramref name="definedSlots"/> using matching slot-fillers
        /// from the specified <paramref name="macroContext"/>.
        /// </summary>
        /// <param name="macroContext">The macro expansion context.</param>
        /// <param name="definedSlots">The defined slots which are available to be filled.</param>
        void FillSlots(MacroExpansionContext macroContext, IEnumerable<Slot> definedSlots);
    }
}
