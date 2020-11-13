using System.Collections.Generic;
using System.Linq;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Factory for the creation of expression contexts.
    /// </summary>
    public class ExpressionContextFactory : IGetsRootExpressionContext, IGetsChildExpressionContexts
    {
        /// <summary>
        /// Gets the root expression context for the request.
        /// </summary>
        /// <returns>The expression context.</returns>
        /// <param name="document">Document.</param>
        /// <param name="request">Request.</param>
        public ExpressionContext GetExpressionContext(IDocument document, RenderZptDocumentRequest request)
            => GetExpressionContext(document.RootElement, document, request.Model, isRoot: true);

        /// <summary>
        /// Gets the child expression contexts.
        /// </summary>
        /// <returns>The expression contexts.</returns>
        /// <param name="context">The context from which to get children.</param>
        public IEnumerable<ExpressionContext> GetChildContexts(ExpressionContext context)
        {
            return context.CurrentElement.ChildNodes
                .Where(x => x.IsElement)
                .Select(x => GetExpressionContext(x, context.TemplateDocument, context.Model, context))
                .ToList();
        }

        ExpressionContext GetExpressionContext(INode element,
                                               IDocument document,
                                               object model,
                                               ExpressionContext parentContext = null,
                                               bool isRoot = false)
        {
            return new ExpressionContext(element,
                                         parentContext?.LocalDefinitions,
                                         parentContext?.GlobalDefinitions,
                                         parentContext?.Repetitions)
            {
                Model = model,
                TemplateDocument = document,
                IsRootContext = isRoot,
            };
        }
    }
}
