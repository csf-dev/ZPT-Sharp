using System;
namespace ZptSharp.Tal
{
    /// <summary>
    /// <para>
    /// Raised when a TAL <c>on-error</c> attribute encounters an exception whilst
    /// attempting to handle a different rendering exception.
    /// </para>
    /// <para>
    /// In this case, the <see cref="Exception.InnerException"/> will relate to the error encountered during
    /// the on-error handling process.  The <see cref="OriginalException"/> is the error (raised in a different
    /// part of the rendering process) which the on-error attribute was attempting to handle.
    /// </para>
    /// </summary>
    [System.Serializable]
    public class OnErrorHandlingException : ZptRenderingException
    {
        static readonly string originalExceptionKey = nameof(OriginalException);

        /// <summary>
        /// Gets or sets the original exception which was being handled when the current exception was raised..
        /// </summary>
        /// <value>The original exception.</value>
        public Exception OriginalException
        {
            get => Data.Contains(originalExceptionKey) ? (Exception) Data[originalExceptionKey] : null;
            set {
                if (value != null)
                    Data[originalExceptionKey] = value;
                else if (Data.Contains(originalExceptionKey))
                    Data.Remove(originalExceptionKey);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OnErrorHandlingException"/> class
        /// </summary>
        public OnErrorHandlingException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OnErrorHandlingException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public OnErrorHandlingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OnErrorHandlingException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public OnErrorHandlingException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OnErrorHandlingException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected OnErrorHandlingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
