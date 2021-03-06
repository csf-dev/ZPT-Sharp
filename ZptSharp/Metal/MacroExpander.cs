﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        readonly ILogger logger;

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

            using (var logScope = logger.BeginScope("Expanding macro {node} ({source_info})", macro.Node, macro.Node.SourceInfo))
            {
                var slotsWhichMacroUsageFills = slotFinder.GetSlotFillers(context.CurrentNode);
                var macroContext = new MacroExpansionContext(macro, slotsWhichMacroUsageFills);
                FillSlotsDefinedByMacro(macroContext, macroContext.Macro);

                return ExpandMacroPrivateAsync(macroContext, context, token);
            }
        }

        async Task<MetalMacro> ExpandMacroPrivateAsync(MacroExpansionContext macroContext,
                                                       ExpressionContext context,
                                                       CancellationToken token)
        {
            var extendedMacro = await macroProvider.GetMacroAsync(macroContext.Macro.Node,
                                                                  context,
                                                                  specProvider.ExtendMacro,
                                                                  token)
                .ConfigureAwait(false);

            if (extendedMacro == null)
            {
                if(logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace("This macro does not extend another", macroContext.Macro.Node);

                return macroContext.Macro;
            }

            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(@"This macro extends another
Extended macro:{extended} ({extended_source})",
                                extendedMacro.Node,
                                extendedMacro.Node.SourceInfo);

            ExtendMacro(macroContext, extendedMacro);
            macroContext.Macro = extendedMacro;
            return await ExpandMacroPrivateAsync(macroContext, context, token)
                .ConfigureAwait(false);
        }

        void ExtendMacro(MacroExpansionContext macroContext, MetalMacro extendedMacro)
        {
            AddSlotFillersToContext(macroContext);
            FillSlotsDefinedByMacro(macroContext, extendedMacro);
        }

        void AddSlotFillersToContext(MacroExpansionContext macroContext)
        {
            var slotsFilledByExtension = slotFinder.GetSlotFillers(macroContext.Macro.Node);

            foreach (var slot in slotsFilledByExtension)
            {
                if (logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace("Found slot-filler in macro extension:{filler}", slot.Node);

                // If we already have a filler for the named slot then we ignore it rather than replacing it.
                // This way consuming templates may always override the filler declared in a 'parent' template.
                if (macroContext.SlotFillers.ContainsKey(slot.Name))
                {
                    if (logger.IsEnabled(LogLevel.Trace))
                        logger.LogTrace("Ignoring slot-filler for {slot_name} because there is already an overriding filler", slot.Name);

                    continue;
                }

                macroContext.SlotFillers.Add(slot.Name, slot);
            }
        }

        void FillSlotsDefinedByMacro(MacroExpansionContext macroContext, MetalMacro slotDefiner)
        {
            var definedSlots = slotFinder.GetDefinedSlots(slotDefiner.Node);
            slotFiller.FillSlots(macroContext, definedSlots);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroExpander"/> class.
        /// </summary>
        /// <param name="macroProvider">Macro provider.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="slotFinder">Slot finder.</param>
        /// <param name="slotFiller">Slot filler.</param>
        /// <param name="logger">A logger.</param>
        public MacroExpander(IGetsMacro macroProvider,
                             IGetsMetalAttributeSpecs specProvider,
                             IGetsSlots slotFinder,
                             IFillsSlots slotFiller,
                             ILogger<MacroExpander> logger)
        {
            this.macroProvider = macroProvider ?? throw new ArgumentNullException(nameof(macroProvider));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.slotFinder = slotFinder ?? throw new ArgumentNullException(nameof(slotFinder));
            this.slotFiller = slotFiller ?? throw new ArgumentNullException(nameof(slotFiller));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
