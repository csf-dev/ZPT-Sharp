using System;
namespace ZptSharp.PathExpressions
{
    /// <summary>
    /// Thrown when an instance of <see cref="IParsesPathExpression"/> fails to parse an instance
    /// of <see cref="PathExpression"/>.
    /// </summary>
    [System.Serializable]
    public class CannotParsePathException : Expressions.EvaluationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CannotParsePathException"/> class
        /// </summary>
        public CannotParsePathException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CannotParsePathException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public CannotParsePathException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CannotParsePathException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public CannotParsePathException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CannotParsePathException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected CannotParsePathException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
