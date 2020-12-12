using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A special case of <see cref="IWalksAndEvaluatesPathExpression"/> which will only permit the use
    /// of global variables and not local or repeat variables, when examining the expression
    /// context for a root object.
    /// </summary>
    public interface IWalksAndEvaluatesPathExpressionWithGlobalVariablesOnly : IWalksAndEvaluatesPathExpression { }
}
