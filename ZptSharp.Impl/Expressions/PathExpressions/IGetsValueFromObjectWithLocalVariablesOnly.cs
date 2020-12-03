using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A special case for <see cref="IGetsValueFromObject"/> which will only permit the use
    /// of local variables and not global or repeat variables, when examining the expression
    /// context for a root object.
    /// </summary>
    public interface IGetsValueFromObjectWithLocalVariablesOnly : IGetsValueFromObject { }
}
