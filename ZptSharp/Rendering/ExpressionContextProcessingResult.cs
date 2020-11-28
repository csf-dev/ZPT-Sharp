using System.Collections.Generic;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Describes the result/outcome of an invocation of <see cref="IProcessesExpressionContext"/>.
    /// </summary>
    public class ExpressionContextProcessingResult
    {
        IList<ExpressionContext> additionalContexts = new List<ExpressionContext>();

        /// <summary>
        /// Gets or sets a collection of additional contexts to be iterated-over.
        /// Typically these contexts are created/added as a result of the processing.
        /// </summary>
        /// <value>The additional contexts.</value>
        public IList<ExpressionContext> AdditionalContexts
        {
            get => additionalContexts;
            set => additionalContexts = value ?? new List<ExpressionContext>();
        }

        /// <summary>
        /// Gets a no-operation instance of <see cref="ExpressionContextProcessingResult"/>.  This result indicates that
        /// context-processing completed without error, but that no particular action needs to be taken as a result.
        /// </summary>
        /// <value>The no-op result.</value>
        public static ExpressionContextProcessingResult Noop => new ExpressionContextProcessingResult();
    }
}
