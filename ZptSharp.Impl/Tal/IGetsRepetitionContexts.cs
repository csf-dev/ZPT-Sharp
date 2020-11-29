using System.Collections.Generic;
using ZptSharp.Expressions;

namespace ZptSharp.Tal
{
    /// <summary>
    /// An object used by <see cref="RepeatAttributeDecorator"/> which gets the collection of
    /// <see cref="ExpressionContext"/> which are exposed by a specified expression result.
    /// </summary>
    public interface IGetsRepetitionContexts
    {
        /// <summary>
        /// Gets the repetition contexts for the specified <paramref name="expressionResult"/>.
        /// </summary>
        /// <returns>The repetition contexts.</returns>
        /// <param name="expressionResult">Expression result.</param>
        /// <param name="sourceContext">Source context.</param>
        /// <param name="repeatVariableName">Repeat variable name.</param>
        IList<ExpressionContext> GetRepetitionContexts(object expressionResult,
                                                       ExpressionContext sourceContext,
                                                       string repeatVariableName);
    }
}
