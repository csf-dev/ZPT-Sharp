namespace ZptSharp.Expressions.PipeExpressions
{
    /// <summary>
    /// Thrown when a TALES pipe expression uses invalid syntax.
    /// </summary>
    [System.Serializable]
    public class CannotParsePipeExpressionException : PipeExpressionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CannotParsePipeExpressionException"/> class
        /// </summary>
        public CannotParsePipeExpressionException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CannotParsePipeExpressionException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        public CannotParsePipeExpressionException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CannotParsePipeExpressionException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public CannotParsePipeExpressionException(string message, System.Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CannotParsePipeExpressionException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected CannotParsePipeExpressionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}