using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Metal
{
    /// <summary>
    /// A model representing an in-progress macro expansion.
    /// </summary>
    public class MacroExpansionContext
    {
        /// <summary>
        /// Gets or sets the macro which is being expanded.
        /// </summary>
        /// <value>The macro.</value>
        public MetalMacro Macro { get; set; }

        /// <summary>
        /// Gets a collection of slot-fillers which remain to be used during macro-expansion.
        /// </summary>
        /// <value>The remaining slot-fillers.</value>
        public IDictionary<string,Slot> SlotFillers { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroExpansionContext"/> class.
        /// </summary>
        /// <param name="macro">Macro.</param>
        /// <param name="slotFillers">An optional initial collection of slot fillers.</param>
        public MacroExpansionContext(MetalMacro macro,
                                     IEnumerable<Slot> slotFillers = null)
        {
            Macro = macro ?? throw new ArgumentNullException(nameof(macro));
            SlotFillers = (slotFillers ?? Enumerable.Empty<Slot>()).ToDictionary(k => k.Name, v => v);
        }
    }
}
