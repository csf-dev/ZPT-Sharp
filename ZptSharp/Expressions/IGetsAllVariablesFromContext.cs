using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which can get all of the variable definitions and their corresponding values
    /// from an <see cref="ExpressionContext"/>.
    /// </summary>
    public interface IGetsAllVariablesFromContext
    {
        /// <summary>
        /// Gets all of the variables &amp; corresponding values from the specified context.
        /// </summary>
        /// <returns>A dictionary of variables and values.</returns>
        /// <param name="context">An expression context.</param>
        Task<IDictionary<string, object>> GetAllVariablesAsync(ExpressionContext context);
    }
}
