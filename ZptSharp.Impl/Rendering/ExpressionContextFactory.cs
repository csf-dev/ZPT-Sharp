using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Factory for the creation of expression contexts.
    /// </summary>
    public class ExpressionContextFactory : IGetsRootExpressionContext, IGetsChildExpressionContexts
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the root expression context for the request.
        /// </summary>
        /// <returns>The expression context.</returns>
        /// <param name="document">Document.</param>
        /// <param name="request">Request.</param>
        public ExpressionContext GetExpressionContext(IDocument document, RenderZptDocumentRequest request)
        {
            var context = GetExpressionContext(document.RootNode, document, request.Model, isRoot: true);
            var config = serviceProvider.GetRequiredService<RenderingConfig>();

            if(config.ContextBuilder != null)
            {
                var helper = new RootContextConfigHelper(context);
                config.ContextBuilder(helper, serviceProvider);
            }

            return context;
        }

        /// <summary>
        /// Gets the child expression contexts.
        /// </summary>
        /// <returns>The expression contexts.</returns>
        /// <param name="context">The context from which to get children.</param>
        public IEnumerable<ExpressionContext> GetChildContexts(ExpressionContext context)
        {
            return context.CurrentNode.ChildNodes
                .Where(x => x.IsElement)
                .Select(x => GetExpressionContext(x, context.TemplateDocument, context.Model, context))
                .ToList();
        }

        ExpressionContext GetExpressionContext(INode node,
                                               IDocument document,
                                               object model,
                                               ExpressionContext parentContext = null,
                                               bool isRoot = false)
        {
            return new ExpressionContext(node,
                                         parentContext?.LocalDefinitions,
                                         parentContext?.GlobalDefinitions,
                                         parentContext?.Repetitions,
                                         parentContext?.ErrorHandlers)
            {
                Model = model,
                TemplateDocument = document,
                IsRootContext = isRoot,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContextFactory"/> class.
        /// </summary>
        /// <param name="serviceProvider">DI service provider.</param>
        public ExpressionContextFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new System.ArgumentNullException(nameof(serviceProvider));
        }
    }
}
