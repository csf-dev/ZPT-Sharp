using System;
namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A special case for <see cref="IGetsValueFromObject"/> which will only permit the use
    /// of global variables and not local or repeat variables, when examining the expression
    /// context for a root object.
    /// </summary>
    public interface IGetsValueFromObjectWithGlobalVariablesOnly : IGetsValueFromObject { }
}
