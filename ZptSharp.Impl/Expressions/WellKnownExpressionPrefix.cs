namespace ZptSharp.Expressions
{
    /// <summary>
    /// A container for runtime constants representing the built-in types of TALES expression.
    /// </summary>
    public static class WellKnownExpressionPrefix
    {
        /// <summary>
        /// The prefix for 'path' expressions.
        /// </summary>
        /// <value>The path prefix.</value>
        public static string Path => "path";

        /// <summary>
        /// The prefix for 'local' expressions (the same as 'path'
        /// expressions but using only a local variable as the start-point for evaluation).
        /// </summary>
        /// <value>The local-path prefix.</value>
        public static string LocalVariablePath => "local";

        /// <summary>
        /// The prefix for 'global' expressions (the same as 'path'
        /// expressions but using only a global variable as the start-point for evaluation).
        /// </summary>
        /// <value>The global-path prefix.</value>
        public static string GlobalVariablePath => "global";

        /// <summary>
        /// The prefix for 'string' expressions.
        /// </summary>
        /// <value>The string prefix.</value>
        public static string String => "string";

        /// <summary>
        /// A shortened alias for 'string' expressions.
        /// </summary>
        /// <value>The string prefix alias.</value>
        public static string ShortStringAlias => "str";

        /// <summary>
        /// The prefix for 'not' expressions.
        /// </summary>
        /// <value>The not prefix.</value>
        public static string Not => "not";
    }
}
