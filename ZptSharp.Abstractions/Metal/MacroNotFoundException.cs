using System;
namespace ZptSharp.Metal
{
    /// <summary>
    /// Thrown when a 'use-macro' attribute indicates that a macro should be used, but the macro to use cannot be found.
    /// </summary>
    [System.Serializable]
    public class MacroNotFoundException : ZptRenderingException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MacroNotFoundException"/> class
        /// </summary>
        public MacroNotFoundException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroNotFoundException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        public MacroNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroNotFoundException"/> class
        /// </summary>
        /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public MacroNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroNotFoundException"/> class
        /// </summary>
        /// <param name="context">The contextual information about the source or destination.</param>
        /// <param name="info">The object that holds the serialized object data.</param>
        protected MacroNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
