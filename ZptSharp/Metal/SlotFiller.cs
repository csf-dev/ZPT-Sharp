using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using ZptSharp.Dom;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IFillsSlots"/> which replaces DOM nodes and removes the slot
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
                    logger.LogTrace("Slot {slot} has no filler in the current context", definedSlot.Node);
                return;
            }

            if (logger.IsEnabled(LogLevel.Trace))
                logger.LogTrace(@"Filling METAL slot with filler
  Slot:{slot}
Filler:{filler}",
                                definedSlot.Node,
                                filler.Node);

            var fillingNode = filler.Node.GetCopy();

            RemoveFillSlotAttributeFromFiller(fillingNode);
            CopyFillSlotAttributeToFiller(fillingNode, definedSlot.Node);

            replacer.Replace(definedSlot.Node, fillingNode);
            macroContext.SlotFillers.Remove(definedSlot.Name);
        }

        /// <summary>
        /// Removes a fill-slot attribute from the node (now that it has been used).
        /// This is important, so as to prevent it accidentally being used again in its new context.
        /// </summary>
        /// <param name="node">The node from which to purge the attribute.</param>
        void RemoveFillSlotAttributeFromFiller(INode node)
        {
            var fillSlotAttribute = node.GetMatchingAttribute(specProvider.FillSlot);
            if (fillSlotAttribute != null)
                node.Attributes.Remove(fillSlotAttribute);
        }

        /// <summary>
        /// If a slot-defining node also fills a slot then that fill-slot attribute needs to
        /// be copied to the slot-filler node.  This is so that it may itself be used to fill
        /// slots (as if it were the defining slot).
        /// </summary>
        /// <param name="fillerNode">The node which is being used to fill a slot.</param>
        /// <param name="definingNode">The node which defined the slot.</param>
        void CopyFillSlotAttributeToFiller(INode fillerNode, INode definingNode)
        {
            var fillSlotAttribute = definingNode.Attributes
                .FirstOrDefault(x => x.Matches(specProvider.FillSlot));

            if (fillSlotAttribute != null)
                fillerNode.Attributes.Add(fillSlotAttribute);
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
