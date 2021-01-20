namespace ZptSharp.BulkRendering
{
    /// <summary>
    /// Raised when an attempt to bulk-render files fails with no files successfully output.
    /// </summary>
    [System.Serializable]
    public class BulkRenderingException : ZptRenderingException
    {
        /// <summary>
        /// Initializes an instance of <see cref="BulkRenderingException" />.
        /// </summary>
        public BulkRenderingException() {}

        /// <summary>
        /// Initializes an instance of <see cref="BulkRenderingException" />.
        /// </summary>
        /// <param name="message">The error message.</param>
        public BulkRenderingException(string message) : base(message) {}

        /// <summary>
        /// Initializes an instance of <see cref="BulkRenderingException" />.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public BulkRenderingException(string message, System.Exception inner) : base(message, inner) {}

        /// <summary>
        /// Initializes an instance of <see cref="BulkRenderingException" /> via serialization.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaming context.</param>
        protected BulkRenderingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}