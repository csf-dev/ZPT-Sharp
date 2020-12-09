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
        readonly ISearchesForAttributes attributeFinder;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            if (context.IsRootContext)
                AddAllMacrosToContext(context);

            return wrapped.ProcessContextAsync(context, token);
        }

        /// <summary>
        /// For ever METAL macro found in the context,
        /// it is added as a global variable.  This should only occur once, when
        /// processing the root expression context.
        /// </summary>
        /// <param name="context">Context.</param>
        void AddAllMacrosToContext(ExpressionContext context)
        {
            var defineMacroAttributes = attributeFinder.SearchForAttributes(context.CurrentElement, specProvider.DefineMacro);

            foreach(var attribute in defineMacroAttributes)
            {
                var macro = new MetalMacro(attribute.Value, attribute.Element.GetCopy());
                context.GlobalDefinitions[macro.Name] = macro;
            }
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AddDefinedMacroToGlobalScopeProcessorDecorator"/> class.
        /// </summary>
        /// <param name="specProvider">Spec provider.</param>
        /// <param name="wrapped">Wrapped service.</param>
        /// <param name="attributeFinder">A service which searches for attributes.</param>
        public AddDefinedMacroToGlobalScopeProcessorDecorator(IGetsMetalAttributeSpecs specProvider,
                                                              IProcessesExpressionContext wrapped,
                                                              ISearchesForAttributes attributeFinder)
        {
            this.specProvider = specProvider ?? throw new ArgumentNullException(nameof(specProvider));
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.attributeFinder = attributeFinder ?? throw new ArgumentNullException(nameof(attributeFinder));
        }
    }
}
