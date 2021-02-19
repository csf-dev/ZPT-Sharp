using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// Raised when an implementation of <see cref="IGetsEvaluatorForExpressionType"/> cannot get an
    /// appropriate evaluator for an expression type.
    /// </summary>
    [System.Serializable]
    public class NoEvaluatorForExpressionTypeException : ZptRenderingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NoEvaluatorForExpressionTypeException"/> class
        /// </summary>
        public NoEvaluatorForExpressionTypeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NoEvaluatorForExpressionTypeException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public NoEvaluatorForExpressionTypeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NoEvaluatorForExpressionTypeException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public NoEvaluatorForExpressionTypeException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NoEvaluatorForExpressionTypeException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected NoEvaluatorForExpressionTypeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
