using System;
namespace ZptSharp.Tal
{
    /// <summary>
    /// Thrown when a TAL define-variable operation fails due to a failure when evaluating the variable value expression.
    /// </summary>
    [Serializable]
    public class DefineVariableEvaluationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DefineVariableException"/> class
        /// </summary>
        public DefineVariableEvaluationException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DefineVariableException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public DefineVariableEvaluationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DefineVariableException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public DefineVariableEvaluationException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DefineVariableException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected DefineVariableEvaluationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
