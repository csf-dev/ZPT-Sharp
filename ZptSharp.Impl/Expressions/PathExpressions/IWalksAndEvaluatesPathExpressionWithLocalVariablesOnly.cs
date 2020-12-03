using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A special case of <see cref="IWalksAndEvaluatesPathExpression"/> which will only permit the use
    /// of local variables and not global or repeat variables, when examining the expression
    /// context for a root object.
    /// </summary>
    public interface IWalksAndEvaluatesPathExpressionWithLocalVariablesOnly : IWalksAndEvaluatesPathExpression { }
}
