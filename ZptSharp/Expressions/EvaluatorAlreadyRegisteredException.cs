using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// Raised when <see cref="IRegistersExpressionEvaluator.RegisterEvaluatorType(Type, string)"/> is used
    /// with an expression type that is already registered.
    /// </summary>
    [System.Serializable]
    public class EvaluatorAlreadyRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:EvaluatorAlreadyRegisteredException"/> class
        /// </summary>
        public EvaluatorAlreadyRegisteredException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EvaluatorAlreadyRegisteredException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public EvaluatorAlreadyRegisteredException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EvaluatorAlreadyRegisteredException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public EvaluatorAlreadyRegisteredException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:EvaluatorAlreadyRegisteredException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected EvaluatorAlreadyRegisteredException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
