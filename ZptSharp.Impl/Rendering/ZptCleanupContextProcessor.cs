using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    public class ZptCleanupContextProcessor : IProcessesExpressionContext
    {
        readonly IGetsWellKnownNamespace namespaceProvider;

        public Task<ExpressionContextProcessingResult> ProcessContextAsync(ExpressionContext context, CancellationToken token = default)
        {
            if (context.CurrentElement.IsInNamespace(namespaceProvider.MetalNamespace)
             || context.CurrentElement.IsInNamespace(namespaceProvider.TalNamespace))
            {
                context.CurrentElement.Omit();
                return Task.FromResult(new ExpressionContextProcessingResult());
            }

            var attributesToRemove = context.CurrentElement.Attributes
                .Where(x => x.IsInNamespace(namespaceProvider.MetalNamespace)
                         || x.IsInNamespace(namespaceProvider.TalNamespace))
                .ToList();

            foreach (var attribute in attributesToRemove)
                context.CurrentElement.Attributes.Remove(attribute);

            return Task.FromResult(new ExpressionContextProcessingResult());
        }

        public ZptCleanupContextProcessor(IGetsWellKnownNamespace namespaceProvider)
        {
            this.namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
        }
    }
}
