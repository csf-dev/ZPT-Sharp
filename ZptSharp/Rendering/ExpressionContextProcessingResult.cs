using System;
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
        public IEnumerable<ExpressionContext> AdditionalContexts { get; set; }
    }
}
