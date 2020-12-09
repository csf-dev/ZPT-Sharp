using System;
namespace ZptSharp.Tal
{
    /// <summary>
    /// Thrown when a TAL attribute value is improperly-formed or otherwise invalid.
    /// </summary>
    [Serializable]
    public class InvalidTalAttributeException : ZptRenderingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTalAttributeException"/> class
        /// </summary>
        public InvalidTalAttributeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTalAttributeException"/> class
        /// </summary>
        /// <param name="message">A <see cref="String"/> that describes the exception. </param>
        public InvalidTalAttributeException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTalAttributeException"/> class
        /// </summary>
        /// <param name="message">A <see cref="String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public InvalidTalAttributeException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidTalAttributeException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected InvalidTalAttributeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
