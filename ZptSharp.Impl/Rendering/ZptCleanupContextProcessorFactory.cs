using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IGetsZptElementAndAttributeRemovalContextProcessor"/> which returns
    /// a context processor suitable for performing cleanup on a rendered document.
    /// </summary>
    public class ZptCleanupContextProcessorFactory : IGetsZptElementAndAttributeRemovalContextProcessor
    {
        readonly IGetsWellKnownNamespace namespaceProvider;

        /// <summary>
        /// Gets the source-annotation context processor.
        /// </summary>
        /// <returns>The source-annotation context processor.</returns>
        public IProcessesExpressionContext GetElementAndAttributeRemovalProcessor()
            => new ZptCleanupContextProcessor(namespaceProvider);

        public ZptCleanupContextProcessorFactory(IGetsWellKnownNamespace namespaceProvider)
        {
            this.namespaceProvider = namespaceProvider ?? throw new System.ArgumentNullException(nameof(namespaceProvider));
        }
    }
}
