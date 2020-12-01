namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An object which is able to parse a path expression string into a model.
    /// </summary>
    public interface IParsesPathExpression
    {
        /// <summary>
        /// Gets a <see cref="PathExpression"/> model object from a specified <paramref name="expression"/> string.
        /// </summary>
        /// <returns>The parsed model object.</returns>
        /// <param name="expression">The expression string.</param>
        /// <exception cref="CannotParsePathException">If the expression cannot be parsed.</exception>
        PathExpression Parse(string expression);
    }
}
