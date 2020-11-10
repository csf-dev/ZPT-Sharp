using System;
using System.Collections.Generic;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IFillsSlots"/> which replaces DOM elements and removes the slot
    /// filler from the macro context when it is used.
    /// </summary>
    public class SlotFiller : IFillsSlots
    {
        /// <summary>
        /// Fills any of the <paramref name="definedSlots"/> using matching slot-fillers
        /// from the specified <paramref name="macroContext"/>.
        /// </summary>
        /// <param name="macroContext">The macro expansion context.</param>
        /// <param name="definedSlots">The defined slots which are available to be filled.</param>
        public void FillSlots(MacroExpansionContext macroContext, IEnumerable<Slot> definedSlots)
        {
            if (macroContext == null)
                throw new ArgumentNullException(nameof(macroContext));
            if (definedSlots == null)
                throw new ArgumentNullException(nameof(definedSlots));

            foreach (var slot in definedSlots)
                Fill(slot, macroContext);
        }

        void Fill(Slot definedSlot, MacroExpansionContext macroContext)
        {
            if (!macroContext.SlotFillers.TryGetValue(definedSlot.Name, out Slot filler)) return;

            definedSlot.Element.ReplaceWith(filler.Element);
            macroContext.SlotFillers.Remove(definedSlot.Name);
        }
    }
}
