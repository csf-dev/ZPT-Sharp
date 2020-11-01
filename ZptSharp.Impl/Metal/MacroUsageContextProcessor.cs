using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        readonly IEvaluatesExpression expressionEvaluator;
        readonly IExpandsMacro macroExpander;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public async Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var useMacroAttribute = context.CurrentElement.Attributes
                .FirstOrDefault(x => x.Matches(specProvider.UseMacro));

            if (useMacroAttribute != null)
                await ReplaceCurrentElementWithExpandedMacroAsync(context, useMacroAttribute.Value, token);

            return new ExpressionContextProcessingResult();
        }

        /// <summary>
        /// Gets the macro using an expression in the use-macro attribute, performs expansion upon it
        /// and then replaces the current DOM element in the expression context with the expanded
        /// macro's root element.
        /// </summary>
        /// <param name="context">The expression context.</param>
        /// <param name="macroExpression">The TALES expression which indicates the macro to use.</param>
        /// <param name="token">A cancellation token.</param>
        async Task ReplaceCurrentElementWithExpandedMacroAsync(ExpressionContext context, string macroExpression, CancellationToken token)
        {
            var macro = await expressionEvaluator.EvaluateExpressionAsync<MetalMacro>(macroExpression, context, token);
            AssertMacroIsNotNull(macro, context, macroExpression);
            var expandedMacro = await macroExpander.ExpandMacroAsync(macro, context, token);
            context.CurrentElement = expandedMacro.Element;
        }

        void AssertMacroIsNotNull(MetalMacro macro, ExpressionContext context, string macroExpression)
        {
            if (macro != null) return;

            var message = String.Format(Resources.ExceptionMessage.MacroNotFound,
                                        specProvider.UseMacro.Name,
                                        context.CurrentElement,
                                        macroExpression);
            throw new MacroNotFoundException(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroUsageContextProcessor"/> class.
        /// </summary>
        /// <param name="specProvider">Attribute spec provider.</param>
        /// <param name="expressionEvaluator">Expresson evaluator.</param>
        /// <param name="macroExpander">Macro expander.</param>
        public MacroUsageContextProcessor(IGetsMetalAttributeSpecs specProvider,
                                          IEvaluatesExpression expressionEvaluator,
                                          IExpandsMacro macroExpander)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.expressionEvaluator = expressionEvaluator ?? throw new ArgumentNullException(nameof(expressionEvaluator));
            this.macroExpander = macroExpander ?? throw new ArgumentNullException(nameof(macroExpander));
        }
    }
}
