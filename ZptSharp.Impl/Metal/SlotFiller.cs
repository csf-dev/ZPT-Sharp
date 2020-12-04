using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IFillsSlots"/> which replaces DOM elements and removes the slot
    /// filler from the macro context when it is used.
    /// </summary>
    public class SlotFiller : IFillsSlots
    {
        readonly ILogger logger;
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly IReplacesNode replacer;

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
            if (!macroContext.SlotFillers.TryGetValue(definedSlot.Name, out Slot filler))
            {
                if (logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace("Slot {slot} has no filler in the current context", definedSlot.Element);
                return;
            }

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace(@"Filling METAL slot with filler
  Slot:{slot}
Filler:{filler}",
                                definedSlot.Element,
                                filler.Element);

            var fillingElement = filler.Element.GetCopy();
            var definingSlotFillerAttribute = GetFillSlotAttributeFromReplacedElement(definedSlot.Element);
            replacer.Replace(definedSlot.Element, fillingElement);
            macroContext.SlotFillers.Remove(definedSlot.Name);
            RemoveFillSlotAttributeFromFiller(fillingElement);
            if (definingSlotFillerAttribute != null)
                fillingElement.Attributes.Add(definingSlotFillerAttribute);
        }

        /// <summary>
        /// Removes a fill-slot attribute from the element.  Important to prevent it accidentally
        /// being used again in its new context.
        /// </summary>
        /// <param name="element">The element from which to purge the attribute.</param>
        void RemoveFillSlotAttributeFromFiller(INode element)
        {
            var fillSlotAttribute = element.GetMatchingAttribute(specProvider.FillSlot);
            if (fillSlotAttribute != null)
                element.Attributes.Remove(fillSlotAttribute);
        }

        IAttribute GetFillSlotAttributeFromReplacedElement(INode element)
        {
            return element.Attributes.FirstOrDefault(x => x.Matches(specProvider.FillSlot));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ZptSharp.Metal.SlotFiller"/> class.
        /// </summary>
        /// <param name="logger">A logger.</param>
        /// <param name="specProvider">Attribute spec provider.</param>
        /// <param name="replacer">A node replacer.</param>
        public SlotFiller(ILogger<SlotFiller> logger,
                          IGetsMetalAttributeSpecs specProvider,
                          IReplacesNode replacer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.replacer = replacer ?? throw new ArgumentNullException(nameof(replacer));
        }
    }
}
