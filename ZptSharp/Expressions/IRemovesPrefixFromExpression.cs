using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which can get a version of a TALES expression where
    /// its expression-type prefix (if any) has been removed.
    /// </summary>
    public interface IRemovesPrefixFromExpression
    {
        /// <summary>
        /// Gets the expression with its expression-type prefix removed (if it had one).
        /// </summary>
        /// <returns>The expression with its prefix removed.</returns>
        /// <param name="expression">An expression which might have a prefix.</param>
        string GetExpressionWithoutPrefix(string expression);
    }
}
