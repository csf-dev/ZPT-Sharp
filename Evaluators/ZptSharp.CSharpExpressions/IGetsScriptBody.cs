namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// An object which gets the body of a C# expression to be compiled.
    /// </summary>
    public interface IGetsScriptBody
    {
        /// <summary>
        /// Gets the script body, from the specified description.
        /// </summary>
        /// <param name="description">An expression description.</param>
        /// <returns>The script body.</returns>
        string GetScriptBody(ExpressionDescription description);
    }
}