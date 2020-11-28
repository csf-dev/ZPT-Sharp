using ZptSharp.Dom;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IGetsZptElementAndAttributeRemovalContextProcessor"/> which returns
    /// a context processor suitable for performing cleanup on a rendered document.
    /// </summary>
    public class ZptCleanupContextProcessorFactory : IGetsZptElementAndAttributeRemovalContextProcessor
    {
        readonly IGetsWellKnownNamespace namespaceProvider;
        readonly Microsoft.Extensions.Logging.ILogger<ZptCleanupContextProcessor> logger;
        readonly IOmitsNode omitter;

        /// <summary>
        /// Gets the source-annotation context processor.
        /// </summary>
        /// <returns>The source-annotation context processor.</returns>
        public IProcessesExpressionContext GetElementAndAttributeRemovalProcessor()
            => new ZptCleanupContextProcessor(namespaceProvider, logger, omitter);

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptCleanupContextProcessorFactory"/> class.
        /// </summary>
        /// <param name="namespaceProvider">Namespace provider.</param>
        /// <param name="logger">A logger.</param>
        public ZptCleanupContextProcessorFactory(IGetsWellKnownNamespace namespaceProvider,
                                                 Microsoft.Extensions.Logging.ILogger<ZptCleanupContextProcessor> logger,
                                                 IOmitsNode omitter)
        {
            this.namespaceProvider = namespaceProvider ?? throw new System.ArgumentNullException(nameof(namespaceProvider));
            this.logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
            this.omitter = omitter ?? throw new System.ArgumentNullException(nameof(omitter));
        }
    }
}
