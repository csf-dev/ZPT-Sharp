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
        /// Fills all of the slots in the specified <paramref name="macro"/> using the
        /// specified <paramref name="fillers"/>.
        /// </summary>
        /// <param name="macro">The macro.</param>
        /// <param name="fillers">Slot fillers.</param>
        void FillSlots(MetalMacro macro, IDictionary<string,SlotFiller> fillers);
    }
}
