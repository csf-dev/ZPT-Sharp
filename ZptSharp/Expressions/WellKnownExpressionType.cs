namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which exposes the names of well-known expression types (which are core to ZPT).
    /// </summary>
    public static class WellKnownExpressionType
    {
        const string
            PathExpression = "path",
            StringExpression = "string";

        /// <summary>
        /// Gets the well-known type name for a path expression.
        /// </summary>
        /// <value>The path expression type.</value>
        public static string Path => PathExpression;

        /// <summary>
        /// Gets the well-known type name for a string expression.
        /// </summary>
        /// <value>The string expression type.</value>
        public static string String => StringExpression;
    }
}
