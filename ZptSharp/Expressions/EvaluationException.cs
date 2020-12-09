using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// Exception raised when there is a problem evaluating an expression.
    /// See also: <seealso cref="IEvaluatesExpression"/>.
    /// </summary>
    [System.Serializable]
    public class EvaluationException : ZptRenderingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationException"/> class
        /// </summary>
        public EvaluationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        public EvaluationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public EvaluationException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected EvaluationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
