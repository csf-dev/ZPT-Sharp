using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IExpandsMacro"/> which first extends the macro and then fills its slots.
    /// </summary>
    public class MacroExpander : IExpandsMacro
    {
        readonly IGetsMacro macroProvider;
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly IGetsSlots slotFinder;
        readonly IFillsSlots slotFiller;

        /// <summary>
        /// Expands the specified macro and returns the result.
        /// </summary>
        /// <returns>The expanded macro.</returns>
        /// <param name="macro">The macro to expand.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<MetalMacro> ExpandMacroAsync(MetalMacro macro, ExpressionContext context, CancellationToken token = default)
        {
            if (macro == null)
                throw new ArgumentNullException(nameof(macro));
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var slotsWhichMacroUsageFills = slotFinder.GetSlotFillers(context.CurrentElement);
            var macroContext = new MacroExpansionContext(macro, slotsWhichMacroUsageFills);
            FillSlotsDefinedByMacro(macroContext, macroContext.Macro);

            return ExpandMacroPrivateAsync(macroContext, context, token);
        }

        async Task<MetalMacro> ExpandMacroPrivateAsync(MacroExpansionContext macroContext,
                                                       ExpressionContext context,
                                                       CancellationToken token)
        {
            var extendedMacro = await macroProvider.GetMacroAsync(macroContext.Macro.Element, context, specProvider.ExtendMacro, token);
            if (extendedMacro == null) return macroContext.Macro;

            ExtendMacro(macroContext, extendedMacro);
            macroContext.Macro = extendedMacro;
            return await ExpandMacroPrivateAsync(macroContext, context, token);
        }

        void ExtendMacro(MacroExpansionContext macroContext, MetalMacro extendedMacro)
        {
            AddSlotFillersToContext(macroContext);
            FillSlotsDefinedByMacro(macroContext, extendedMacro);
        }

        void AddSlotFillersToContext(MacroExpansionContext macroContext)
        {
            var slotsFilledByExtension = slotFinder.GetSlotFillers(macroContext.Macro.Element);

            foreach (var slot in slotsFilledByExtension)
            {
                if (macroContext.SlotFillers.ContainsKey(slot.Name))
                    macroContext.SlotFillers.Remove(slot.Name);

                macroContext.SlotFillers.Add(slot.Name, slot);
            }
        }

        void FillSlotsDefinedByMacro(MacroExpansionContext macroContext, MetalMacro slotDefiner)
        {
            var definedSlots = slotFinder.GetDefinedSlots(slotDefiner.Element);
            slotFiller.FillSlots(macroContext, definedSlots);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroExpander"/> class.
        /// </summary>
        /// <param name="macroProvider">Macro provider.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="slotFinder">Slot finder.</param>
        /// <param name="slotFiller">Slot filler.</param>
        public MacroExpander(IGetsMacro macroProvider,
                             IGetsMetalAttributeSpecs specProvider,
                             IGetsSlots slotFinder,
                             IFillsSlots slotFiller)
        {
            this.macroProvider = macroProvider ?? throw new ArgumentNullException(nameof(macroProvider));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.slotFinder = slotFinder ?? throw new ArgumentNullException(nameof(slotFinder));
            this.slotFiller = slotFiller ?? throw new ArgumentNullException(nameof(slotFiller));
        }
    }
}
