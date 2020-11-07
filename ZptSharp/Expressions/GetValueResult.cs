namespace ZptSharp.Expressions
{
    /// <summary>
    /// Represents the outcome of <see cref="IGetsNamedTalesValue.TryGetValueAsync(string)"/>
    /// </summary>
    public struct GetValueResult
    {
        /// <summary>
        /// Gets a value indicating whether a value was successfully retrieved or not.
        /// </summary>
        /// <value><c>true</c> if a value was retrieved; otherwise, <c>false</c>.</value>
        public bool Success { get; }

        /// <summary>
        /// Gets the value which was retrieved, or a <see langword="null"/> reference if <see cref="Success"/> is <c>false</c>.
        /// </summary>
        /// <value>The retrieved result.</value>
        public object Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetValueResult"/> struct.
        /// </summary>
        /// <param name="success">If set to <c>true</c> then this instance indicates success.</param>
        /// <param name="result">The retrieved result.</param>
        public GetValueResult(bool success, object result)
        {
            Success = success;
            Value = result;
        }

        /// <summary>
        /// Gets a <see cref="GetValueResult"/> which indicates failure to retrieve a result.
        /// </summary>
        /// <value>A failure result.</value>
        public static GetValueResult Failure => new GetValueResult(false, null);

        /// <summary>
        /// Gets a <see cref="GetValueResult"/> which indicates that the specified
        /// <paramref name="result"/> object was successfully retrieved.
        /// </summary>
        /// <returns>A get-value result.</returns>
        /// <param name="result">The result object.</param>
        public static GetValueResult For(object result) => new GetValueResult(true, result);
    }
}
