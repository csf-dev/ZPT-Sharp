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
        /// Gets a value indicating whether this <see cref="ExpressionContextProcessingResult"/>
        /// indicates that children of the current node should not be processed.
        /// </summary>
        /// <value><c>true</c> if child nodes should not be processed; otherwise, <c>false</c>.</value>
        public bool DoNotProcessChildren { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContextProcessingResult"/> class.
        /// </summary>
        public ExpressionContextProcessingResult() : this(false) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionContextProcessingResult"/> class.
        /// </summary>
        /// <param name="doNotProcessChildren">If set to <c>true</c> the child contexts should not be processed.</param>
        ExpressionContextProcessingResult(bool doNotProcessChildren)
        {
            DoNotProcessChildren = doNotProcessChildren;
        }

        /// <summary>
        /// Gets a no-operation instance of <see cref="ExpressionContextProcessingResult"/>.  This result indicates that
        /// context-processing completed without error, but that no particular action needs to be taken as a result.
        /// </summary>
        /// <value>The no-op result.</value>
        public static ExpressionContextProcessingResult Noop => new ExpressionContextProcessingResult();

        /// <summary>
        /// Gets an instance of <see cref="ExpressionContextProcessingResult"/> which indicates that child-contexts
        /// should not be processed.
        /// </summary>
        /// <value>The processing result.</value>
        public static ExpressionContextProcessingResult WithoutChildren => new ExpressionContextProcessingResult(true);
    }
}
