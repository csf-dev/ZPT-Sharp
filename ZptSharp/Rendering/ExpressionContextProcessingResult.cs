using System.Collections.Generic;
using ZptSharp.Expressions;

namespace ZptSharp.Rendering
{
    /// <summary>
    /// Describes the result/outcome of an invocation of <see cref="IProcessesExpressionContext"/>.
    /// </summary>
    public class ExpressionContextProcessingResult
    {
        /// <summary>
        /// Gets or sets a collection of additional contexts to be iterated-over.
        /// Typically these contexts are created/added as a result of the processing.
        /// </summary>
        /// <value>The additional contexts.</value>
        public IList<ExpressionContext> AdditionalContexts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current expression context should
        /// continue to be processed.  If this property returns <see langword="false"/> then
        /// logic should abort processing.  Typically this means that the DOM element within
        /// an <see cref="ExpressionContext"/> has been removed from the document and so further
        /// processing is irrelevant.
        /// </summary>
        /// <value><c>true</c> if further processing should be aborted; otherwise, <c>false</c>.</value>
        public bool AbortFurtherProcessing { get; set; }

        /// <summary>
        /// Gets a no-operation instance of <see cref="ExpressionContextProcessingResult"/>.  This result indicates that
        /// context-processing completed without error, but that no particular action needs to be taken as a result.
        /// </summary>
        /// <value>The no-op result.</value>
        public static ExpressionContextProcessingResult Noop => new ExpressionContextProcessingResult();
    }
}
