using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        readonly ILogger logger;
        readonly IReplacesNode replacer;

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
                                                          new[] { specProvider.UseMacro, specProvider.ExtendMacro },
                                                          token)
                .ConfigureAwait(false);

            if (macro != null)
            {
                await ReplaceCurrentElementWithExpandedMacroAsync(context, macro, token)
                    .ConfigureAwait(false);
            }

            return ExpressionContextProcessingResult.Noop;
        }

        async Task ReplaceCurrentElementWithExpandedMacroAsync(ExpressionContext context, MetalMacro macro, CancellationToken token)
        {
            var expandedMacro = await macroExpander.ExpandMacroAsync(macro, context, token)
                .ConfigureAwait(false);

            if(logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug(@"Replacing use-macro element with expanded macro
Macro element:{macro_element} ({macro_element_source})
Using element:{macro_user} ({macro_user_source})",
                                expandedMacro.Element,
                                expandedMacro.Element.SourceInfo,
                                context.CurrentElement,
                                context.CurrentElement.SourceInfo);

            var replacement = expandedMacro.Element;
            replacer.Replace(context.CurrentElement, replacement);
            context.CurrentElement = replacement;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroUsageContextProcessor"/> class.
        /// </summary>
        /// <param name="specProvider">Attribute spec provider.</param>
        /// <param name="macroProvider">Macro provider.</param>
        /// <param name="macroExpander">Macro expander.</param>
        /// <param name="logger">A logger.</param>
        /// <param name="replacer">The node replacer.</param>
        public MacroUsageContextProcessor(IGetsMetalAttributeSpecs specProvider,
                                          IGetsMacro macroProvider,
                                          IExpandsMacro macroExpander,
                                          ILogger<MacroUsageContextProcessor> logger,
                                          IReplacesNode replacer)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.macroProvider = macroProvider ?? throw new ArgumentNullException(nameof(macroProvider));
            this.macroExpander = macroExpander ?? throw new ArgumentNullException(nameof(macroExpander));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.replacer = replacer ?? throw new ArgumentNullException(nameof(replacer));
        }
    }
}