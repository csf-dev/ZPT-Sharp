using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Rendering;

namespace ZptSharp.Metal
{
    /// <summary>
    /// Decorator for the <see cref="IProcessesExpressionContext"/> service which detects elements
    /// which have 'define-macro' attributes upon them.  If such an attribute is found then the
    /// macro is stored in the current context's global scope for later usage.
    /// </summary>
    public class AddDefinedMacroToGlobalScopeProcessorDecorator : IProcessesExpressionContext
    {
        readonly IGetsMetalAttributeSpecs specProvider;
        readonly IProcessesExpressionContext wrapped;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var macro = GetMacro(context);
            if (macro != null) context.GlobalDefinitions[macro.Name] = macro;
            return wrapped.ProcessContextAsync(context, token);
        }

        MetalMacro GetMacro(ExpressionContext context)
        {
            var defineMacroAttribute = context.CurrentElement.GetMatchingAttribute(specProvider.DefineMacro);
            if (defineMacroAttribute == null) return null;
            return new MetalMacro(defineMacroAttribute.Value, context.CurrentElement);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AddDefinedMacroToGlobalScopeProcessorDecorator"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="wrapped">Wrapped service.</param>
        public AddDefinedMacroToGlobalScopeProcessorDecorator(IGetsMetalAttributeSpecs specProvider,
                                                              IProcessesExpressionContext wrapped)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
