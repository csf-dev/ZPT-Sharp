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
        /// The prefix for 'local' expressions.
        /// The are the same as 'path' expressions but using only local
        /// variables as the start-point for evaluation.
        /// </summary>
        /// <value>The local-path prefix.</value>
        public static string LocalVariablePath => "local";

        /// <summary>
        /// The prefix for 'global' expressions.
        /// The are the same as 'path' expressions but using only global
        /// variables as the start-point for evaluation.
        /// </summary>
        /// <value>The global-path prefix.</value>
        public static string GlobalVariablePath => "global";

        /// <summary>
        /// The prefix for 'var' expressions.
        /// The are the same as 'path' expressions but using only defined (local or global)
        /// variables as the start-point for evaluation.
        /// </summary>
        /// <value>The local-path prefix.</value>
        public static string DefinedVariablePath => "var";

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

        /// <summary>
        /// The prefix for 'pipe' expressions.
        /// </summary>
        /// <value>The pipe prefix.</value>
        public static string Pipe => "pipe";
    }
}
