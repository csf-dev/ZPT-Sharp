using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// A container for runtime constants representing the built-in types of TALES expression.
    /// </summary>
    public sealed class WellKnownExpressionPrefix
    {
        /// <summary>
        /// The prefix for 'path' expressions.
        /// </summary>
        /// <value>The path prefix.</value>
        public static string Path => "path";

        /// <summary>
        /// The prefix for 'string' expressions.
        /// </summary>
        /// <value>The string prefix.</value>
        public static string String => "string";

        /// <summary>
        /// The prefix for 'not' expressions.
        /// </summary>
        /// <value>The not prefix.</value>
        public static string Not => "not";
    }
}
