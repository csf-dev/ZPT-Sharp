using System.Collections.Generic;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// A compiled C# expression is a delegate which accepts a dictionary of named variables
    /// that are in-scope and returns an object.
    /// </summary>
    /// <param name="variables">A collection of the named variables which are in-scope for the expression.</param>
    /// <returns>The result of the expression evaluation.</returns>
    public delegate object CSharpExpression(IDictionary<string,object> variables);
}