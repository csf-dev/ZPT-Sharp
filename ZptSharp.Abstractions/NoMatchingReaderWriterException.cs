using System;
namespace ZptSharp
{
    /// <summary>
    /// Raised when no suitable implementation of <see cref="Dom.IReadsAndWritesDocument"/> can be found,
    /// but an implementation is required in order to proceed.
    /// </summary>
    [System.Serializable]
    public class NoMatchingReaderWriterException : ZptRenderingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoMatchingReaderWriterException"/> class
        /// </summary>
        public NoMatchingReaderWriterException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoMatchingReaderWriterException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        public NoMatchingReaderWriterException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoMatchingReaderWriterException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public NoMatchingReaderWriterException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoMatchingReaderWriterException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected NoMatchingReaderWriterException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
