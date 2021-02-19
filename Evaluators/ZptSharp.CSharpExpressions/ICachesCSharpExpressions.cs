namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// An object which caches compiled <see cref="CSharpExpression" /> instances,
    /// based upon their identifiers.
    /// </summary>
    public interface ICachesCSharpExpressions
    {
        /// <summary>
        /// Gets a compiled C# expression from the cache, or a <see langword="null" />
        /// reference if there is no expression in the cache matching the identifier.
        /// </summary>
        /// <param name="description">An identifier for a compiled C# expression.</param>
        /// <returns>A C# expression, or a <see langword="null" /> reference if the expression is not found.</returns>
        CSharpExpression GetExpression(ExpressionDescription description);

        /// <summary>
        /// Adds a compiled C# expression to the cache.
        /// </summary>
        /// <param name="description">An object which uniquely identifies the expression.</param>
        /// <param name="expression">The compiled C# expression.</param>
        void AddExpression(ExpressionDescription description, CSharpExpression expression);
    }
}