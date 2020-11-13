using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// An implementation of <see cref="IProcessesExpressionContext"/> which 'cleans up'
    /// ZPT markup from the rendered document.  This context processor would typically
    /// be executed last, in order to strip out all ZPT-related directives from a document
    /// which has already been processed.
    /// </summary>
    public class ZptCleanupContextProcessor : IProcessesExpressionContext
    {
        readonly IGetsWellKnownNamespace namespaceProvider;

        /// <summary>
        /// Processes the context using the rules defined within this object.
        /// </summary>
        /// <returns>A result object indicating the outcome of processing.</returns>
        /// <param name="context">The context to process.</param>
        /// <param name="token">An optional cancellation token.</param>
        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            if (NeedsCleanup(context.CurrentElement))
            {
                context.CurrentElement.Omit();
                return Task.FromResult(new ExpressionContextProcessingResult());
            }

            var attributesToRemove = context.CurrentElement.Attributes
                .Where(NeedsCleanup)
                .ToList();

            foreach (var attribute in attributesToRemove)
                context.CurrentElement.Attributes.Remove(attribute);

            return Task.FromResult(new ExpressionContextProcessingResult());
        }

        bool NeedsCleanup(INode element)
        {
            return element.IsInNamespace(namespaceProvider.MetalNamespace)
                || element.IsInNamespace(namespaceProvider.TalNamespace);
        }

        bool NeedsCleanup(IAttribute attribute)
        {
            return attribute.IsInNamespace(namespaceProvider.MetalNamespace)
                || attribute.IsInNamespace(namespaceProvider.TalNamespace);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptCleanupContextProcessor"/> class.
        /// </summary>
        /// <param name="namespaceProvider">Namespace provider.</param>
        public ZptCleanupContextProcessor(IGetsWellKnownNamespace namespaceProvider)
        {
            this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        }
    }
}
