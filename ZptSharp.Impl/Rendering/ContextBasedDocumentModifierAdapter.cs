using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Implementation of <see cref="IModifiesDocumentUsingContext"/> which really just
    /// wraps a <see cref="IModifiesDocument"/> and presents an alternative API.
    /// </summary>
    public class ContextBasedDocumentModifierAdapter : IModifiesDocumentUsingContext
    {
        readonly IModifiesDocument wrapped;

        /// <summary>
        /// Performs alterations for a specified document, using the specified expression context.
        /// This method will manipulate the <paramref name="context"/>, according to the rules of ZPT.
        /// </summary>
        /// <returns>A task indicating when the process is complete.</returns>
        /// <param name="context">The expression context to use in the operation.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        public Task ModifyDocumentAsync(ExpressionContext context, CancellationToken token = default)
            => wrapped.ModifyDocumentAsync(context.TemplateDocument, context.Model, token);

        /// <summary>
        /// Initializes a new instance of <see cref="ContextBasedDocumentModifierAdapter"/>.
        /// </summary>
        /// <param name="wrapped">The wrapped modifier implementation.</param>
        public ContextBasedDocumentModifierAdapter(IModifiesDocument wrapped)
        {
            this.wrapped = wrapped ?? throw new System.ArgumentNullException(nameof(wrapped));

        }
    }
}