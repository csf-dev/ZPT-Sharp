namespace ZptSharp.Expressions.PipeExpressions
{
    /// <summary>
    /// Thrown when a TALES pipe expression raises an exception during its evaluation.
    /// </summary>
    public class PipeExpressionException : EvaluationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PipeExpressionException"/> class
        /// </summary>
        public PipeExpressionException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeExpressionException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        public PipeExpressionException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeExpressionException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public PipeExpressionException(string message, System.Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="PipeExpressionException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected PipeExpressionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}