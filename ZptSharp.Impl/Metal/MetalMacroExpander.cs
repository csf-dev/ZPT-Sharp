using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using ZptSharp.Dom;
using System.Linq;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Implementation of <see cref="IExpandsMacro"/> which first extends the macro and then fills its slots.
    /// </summary>
    public class MetalMacroExpander : IExpandsMacro
    {
        readonly IExtendsMacro macroExtender;
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly ISearchesForAttributes attributeFinder;
        readonly IFillsSlots slotFiller;

        /// <summary>
        /// Expands the specified macro and returns the result.
        /// </summary>
        /// <returns>The expanded macro.</returns>
        /// <param name="macro">The macro to expand.</param>
        /// <param name="context">The current expression context.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<MetalMacro> ExpandMacroAsync(MetalMacro macro, ExpressionContext context, CancellationToken token = default)
        {
            var slotFillers = attributeFinder
                .SearchForAttributes(macro.Element, specProvider.FillSlot)
                .ToDictionary(k => k.Value, v => new SlotFiller(v.Value, v.Element));

            var extendedMacro = await macroExtender.ExtendMacroAsync(macro, context, slotFillers, token);
            slotFiller.FillSlots(extendedMacro, slotFillers);

            return extendedMacro;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MetalMacroExpander"/> class.
        /// </summary>
        /// <param name="macroExtender">Macro extender.</param>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="attributeFinder">Attribute finder.</param>
        /// <param name="slotFiller">Slot filler.</param>
        public MetalMacroExpander(IExtendsMacro macroExtender,
                                  IGetsMetalAttributeSpecs specProvider,
                                  ISearchesForAttributes attributeFinder,
                                  IFillsSlots slotFiller)
        {
            this.macroExtender = macroExtender ?? throw new ArgumentNullException(nameof(macroExtender));
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.attributeFinder = attributeFinder ?? throw new ArgumentNullException(nameof(attributeFinder));
            this.slotFiller = slotFiller ?? throw new ArgumentNullException(nameof(slotFiller));
        }
    }
}
