using System;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Thrown when a TALES csharp expression raises an exception during its evaluation.
    /// </summary>
    [Serializable]
    public class CSharpEvaluationException : EvaluationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpEvaluationException"/> class
        /// </summary>
        public CSharpEvaluationException() {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpEvaluationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        public CSharpEvaluationException(string message) : base(message) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpEvaluationException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public CSharpEvaluationException(string message, System.Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpEvaluationException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected CSharpEvaluationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}