using System;
namespace ZptSharp
{
    /// <summary>
    /// Base class for an exception raised by the ZPT Sharp rendering process.
    /// </summary>
    [System.Serializable]
    public class ZptRenderingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ZptRenderingException"/> class
        /// </summary>
        public ZptRenderingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptRenderingException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public ZptRenderingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptRenderingException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public ZptRenderingException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ZptRenderingException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected ZptRenderingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
