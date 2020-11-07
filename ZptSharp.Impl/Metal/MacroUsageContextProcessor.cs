using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Metal
{
    /// <summary>
    /// A <see cref="IProcessesExpressionContext"/> which applies the METAL rendering process.
    /// This begins with detection of the <c>metal:use-macro</c> attribute.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <c>metal:use-macro</c> attribute indicates that the current DOM element should be replaced by an
    /// expanded METAL macro.  The macro to use as the replacement is indicated via an expression which is the value
    /// of the use-macro attribute.
    /// </para>
    /// <para>
    /// Macro expansion is handled by the service <see cref="IExpandsMacro"/>.  This includes both filling slots
    /// which are defined in the used macro and also performing macro extension, if applicable.
    /// </para>
    /// </remarks>
    public class MacroUsageContextProcessor : IProcessesExpressionContext
    {
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly IGetsMacro macroProvider;
        readonly IExpandsMacro macroExpander;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var macro = await macroProvider.GetMacroAsync(context.CurrentElement,
                                                          context,
                                                          specProvider.UseMacro,
                                                          token)
                .ConfigureAwait(false);

            if (macro != null)
            {
                await ReplaceCurrentElementWithExpandedMacroAsync(context, macro, token)
                    .ConfigureAwait(false);
            }

            return new ExpressionContextProcessingResult();
        }

        async Task ReplaceCurrentElementWithExpandedMacroAsync(ExpressionContext context, MetalMacro macro, CancellationToken token)
        {
            var expandedMacro = await macroExpander.ExpandMacroAsync(macro, context, token)
                .ConfigureAwait(false);
            context.CurrentElement = expandedMacro.Element;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroUsageContextProcessor"/> class.
        /// </summary>
        /// <param name="specProvider">Attribute spec provider.</param>
        /// <param name="macroProvider">Macro provider.</param>
        /// <param name="macroExpander">Macro expander.</param>
        public MacroUsageContextProcessor(IGetsMetalAttributeSpecs specProvider,
                                          IGetsMacro macroProvider,
                                          IExpandsMacro macroExpander)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.macroProvider = macroProvider ?? throw new ArgumentNullException(nameof(macroProvider));
            this.macroExpander = macroExpander ?? throw new ArgumentNullException(nameof(macroExpander));
        }
    }
}
