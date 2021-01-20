using System;
namespace ZptSharp.Tal
{
    /// <summary>
    /// A wrapper/adapter class for instances of <see cref="Exception"/> which are added to
    /// the expression context by an <see cref="OnErrorAttributeDecorator"/>.
    /// This exposes a standard API, which is part of the TAL spec.
    /// </summary>
    public class OnErrorExceptionAdapter
    {
        /// <summary>
        /// Gets the exception type.
        /// </summary>
        /// <value>The exception type.</value>
        public Type type => value.GetType();

        /// <summary>
        /// Gets the exception object instance
        /// </summary>
        /// <value>The exception object.</value>
        public Exception value { get; }

        /// <summary>
        /// Gets the exception traceback (AKA stack trace).
        /// </summary>
        /// <value>The stack trace.</value>
        public string traceback => value.StackTrace;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnErrorExceptionAdapter"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public OnErrorExceptionAdapter(Exception exception)
        {
            value = exception ?? throw new ArgumentNullException(nameof(exception));
        }
    }
}
