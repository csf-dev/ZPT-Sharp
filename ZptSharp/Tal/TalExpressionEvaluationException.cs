using System;
namespace ZptSharp.Tal
{
    /// <summary>
    /// Thrown when a TAL attribute operation fails due to a error when evaluating a value expression.
    /// </summary>
    [Serializable]
    public class TalExpressionEvaluationException : ZptRenderingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TalExpressionEvaluationException"/> class
        /// </summary>
        public TalExpressionEvaluationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TalExpressionEvaluationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public TalExpressionEvaluationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TalExpressionEvaluationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public TalExpressionEvaluationException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TalExpressionEvaluationException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected TalExpressionEvaluationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
