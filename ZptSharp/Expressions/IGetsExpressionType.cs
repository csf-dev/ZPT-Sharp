using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which can determine the string identifier which indicates an expression type.
    /// This is typically a prefix to the expression body.
    /// </summary>
    public interface IGetsExpressionType
    {
        /// <summary>
        /// Gets the expression type for the specified <paramref name="expression"/>.
        /// </summary>
        /// <returns>The expression type.</returns>
        /// <param name="expression">An expression.</param>
        string GetExpressionType(string expression);
    }
}
