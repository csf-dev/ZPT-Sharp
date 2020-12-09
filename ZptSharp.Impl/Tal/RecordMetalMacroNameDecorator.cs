using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.Tal
{
    /// <summary>
    /// A decorator for <see cref="IHandlesProcessingError"/> which detects METAL <c>define-macro</c>
    /// attributes and adds a TALES local variable indicating the name of that macro.  This is recorded as
    /// the <c>macroname</c> variable.
    /// </summary>
    public class RecordMetalMacroNameDecorator : IHandlesProcessingError
    {
        const string macroNameVariable = "macroname";

        readonly IHandlesProcessingError wrapped;
        readonly IGetsMetalAttributeSpecs metalSpecProvider;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            var attribute = context.CurrentElement.GetMatchingAttribute(metalSpecProvider.DefineMacro);
            if (attribute != null)
                context.LocalDefinitions[macroNameVariable] = attribute.Value;

            return wrapped.ProcessContextAsync(context, token);
        }

        Task<ErrorHandlingResult> IHandlesProcessingError.HandleErrorAsync(Exception ex, ExpressionContext context, CancellationToken token)
            => wrapped.HandleErrorAsync(ex, context, token);

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordMetalMacroNameDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="metalSpecProvider">Metal spec provider.</param>
        public RecordMetalMacroNameDecorator(IHandlesProcessingError wrapped,
                                             IGetsMetalAttributeSpecs metalSpecProvider)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.metalSpecProvider = metalSpecProvider ?? throw new ArgumentNullException(nameof(metalSpecProvider));
        }
    }
}
