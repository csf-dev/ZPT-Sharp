using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        readonly ILogger logger;
        readonly IOmitsNode omitter;

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
                if(logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace($"Removing {{element}}",
                                    context.CurrentElement);
                omitter.Omit(context.CurrentElement);
                return Task.FromResult(ExpressionContextProcessingResult.Noop);
            }

            var attributesToRemove = context.CurrentElement.Attributes
                .Where(NeedsCleanup)
                .ToList();

            foreach (var attribute in attributesToRemove)
            {
                if (logger.IsEnabled(LogLevel.Trace))
                    logger.LogTrace("Removing attribute \"{attribute}\" from {element}",
                                    attribute.Name,
                                    context.CurrentElement);
                context.CurrentElement.Attributes.Remove(attribute);
            }

            return Task.FromResult(ExpressionContextProcessingResult.Noop);
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
        /// <param name="logger">A logger.</param>
        /// <param name="omitter">The node omitter.</param>
        public ZptCleanupContextProcessor(IGetsWellKnownNamespace namespaceProvider,
                                          ILogger<ZptCleanupContextProcessor> logger,
                                          IOmitsNode omitter)
        {
            this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.omitter = omitter ?? throw new ArgumentNullException(nameof(omitter));
        }
    }
}
