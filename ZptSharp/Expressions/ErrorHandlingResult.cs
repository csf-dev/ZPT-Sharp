using System;
using ZptSharp.Rendering;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Gets the result of an attempt to handle an error.
    /// </summary>
    public class ErrorHandlingResult
    {
        /// <summary>
        /// Gets the result of the error-handling attempt.
        /// </summary>
        /// <value>The result.</value>
        public ExpressionContextProcessingResult Result { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:ZptSharp.Expressions.ErrorHandlingResult"/> was a success.
        /// </summary>
        /// <value><c>true</c> if the attempt was a success; otherwise, <c>false</c>.</value>
        public bool IsSuccess => Result != null;

        ErrorHandlingResult(ExpressionContextProcessingResult result)
        {
            Result = result;
        }

        /// <summary>
        /// Gets an instance which represents successfully handling an error.
        /// </summary>
        /// <returns>The success result.</returns>
        /// <param name="result">Result.</param>
        public static ErrorHandlingResult Success(ExpressionContextProcessingResult result)
            => new ErrorHandlingResult(result ?? throw new ArgumentNullException(nameof(result)));

        /// <summary>
        /// Gets an instance which represents an unsuccessful attempt to handle an error.
        /// </summary>
        /// <returns>The failure result.</returns>
        public static ErrorHandlingResult Failure() => new ErrorHandlingResult(null);
    }
}
