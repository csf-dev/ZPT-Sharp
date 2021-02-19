using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PythonExpressions
{
    /// <summary>
    /// An object which can evaluate a string python expression, given a dictionary of
    /// variable definitions, forming the execution context.
    /// </summary>
    public interface IEvaluatesPythonExpression
    {
        /// <summary>
        /// Evaluates the python expression and returns the result.
        /// </summary>
        /// <returns>The evaluated expression.</returns>
        /// <param name="expression">Expression.</param>
        /// <param name="variableDefinitions">Variable definitions.</param>
        /// <param name="token">Token.</param>
        Task<object> EvaluateExpressionAsync(string expression, IList<Variable> variableDefinitions, CancellationToken token = default);
    }
}
