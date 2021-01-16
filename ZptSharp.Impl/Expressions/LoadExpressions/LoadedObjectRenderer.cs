using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Dom;
using ZptSharp.Metal;
using ZptSharp.Rendering;

namespace ZptSharp.Expressions.LoadExpressions
{
    /// <summary>
    /// Implementation of <see cref="IRendersLoadedObject"/> which gets a string that represents the 
    /// </summary>
    public class LoadedObjectRenderer : IRendersLoadedObject
    {
        static readonly Type[] supportedTypes = {
            typeof(IDocument),
            typeof(INode),
            typeof(MetalDocumentAdapter),
            typeof(MetalMacro),
        };

        readonly IGetsDocumentModifier documentModifierFactory;

        /// <summary>
        /// Renders the specified object asynchronously and returns the result.
        /// </summary>
        /// <param name="obj">The object to render.</param>
        /// <param name="context">The expression context.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>The result of the rendering operation.</returns>
        public Task<string> RenderObjectAsync(object obj, ExpressionContext context, CancellationToken cancellationToken = default)
        {
            if (obj is null) return Task.FromResult<string>(null);
            if (context is null)
                throw new System.ArgumentNullException(nameof(context));

            return RenderObjectPrivateAsync(obj, context, cancellationToken);
        }

        async Task<string> RenderObjectPrivateAsync(object obj, ExpressionContext context, CancellationToken cancellationToken)
        {
            if (obj is IDocument document) return await RenderDocumentAsync(document, context, cancellationToken);
            if (obj is MetalDocumentAdapter metalDocument) return await RenderDocumentAsync(metalDocument.Document, context, cancellationToken);
            if (obj is MetalMacro macro) return await RenderNodeAsync(macro.Node, context, cancellationToken);
            if (obj is INode node) return await RenderNodeAsync(node, context, cancellationToken);

            var message = String.Format(Resources.ExceptionMessage.UnsupportedLoadExpressionResult,
                                        context.CurrentNode,
                                        obj.GetType().FullName,
                                        String.Join(", ", supportedTypes.Select(x => x.Name)));
            throw new EvaluationException(message);
        }

        async Task<string> RenderDocumentAsync(IDocument document, ExpressionContext context, CancellationToken cancellationToken)
        {
            var docModifier = documentModifierFactory.GetDocumentModifierUsingContext();
            await docModifier.ModifyDocumentAsync(context, cancellationToken);

            throw new NotImplementedException();
        }

        Task<string> RenderNodeAsync(INode node, ExpressionContext context, CancellationToken cancellationToken)
        {
            var doc = new DocumentFromNodeAdapter(node);
            return RenderDocumentAsync(doc, context, cancellationToken);
        }

        public LoadedObjectRenderer(IGetsDocumentModifier documentModifierFactory)
        {
            this.documentModifierFactory = documentModifierFactory ?? throw new ArgumentNullException(nameof(documentModifierFactory));

        }
    }
}