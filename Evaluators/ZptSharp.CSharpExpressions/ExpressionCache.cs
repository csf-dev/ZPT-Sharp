using System;
using System.Runtime.Caching;

namespace ZptSharp.Expressions.CSharpExpressions
{
    /// <summary>
    /// Implementation of <see cref="ICachesCSharpExpressions" /> which uses a
    /// <see cref="MemoryCache" /> as the backing store.
    /// </summary>
    public class ExpressionCache : ICachesCSharpExpressions
    {
        readonly ObjectCache cache = new MemoryCache($"{typeof(ExpressionCache).FullName}_cache");

        /// <summary>
        /// Adds a compiled C# expression to the cache.
        /// </summary>
        /// <param name="description">An object which uniquely identifies the expression.</param>
        /// <param name="expression">The compiled C# expression.</param>
        public void AddExpression(ExpressionDescription description, CSharpExpression expression)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            var cacheKey = description.ToString();
            cache.Set(cacheKey, expression, DateTimeOffset.MaxValue);
        }

        /// <summary>
        /// Gets a compiled C# expression from the cache, or a <see langword="null" />
        /// reference if there is no expression in the cache matching the identifier.
        /// </summary>
        /// <param name="description">An identifier for a compiled C# expression.</param>
        /// <returns>A C# expression, or a <see langword="null" /> reference if the expression is not found.</returns>
        public CSharpExpression GetExpression(ExpressionDescription description)
        {
            if (description is null)
                throw new ArgumentNullException(nameof(description));

            var cacheKey = description.ToString();
            return cache.Get(cacheKey) as CSharpExpression;
        }
    }
}