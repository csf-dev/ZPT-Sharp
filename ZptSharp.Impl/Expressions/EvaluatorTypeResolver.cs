using System;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IGetsEvaluatorForExpressionType"/> which uses a
    /// <see cref="IServiceProvider"/> to resolve expression evaluators.
    /// </summary>
    public class EvaluatorTypeResolver : IGetsEvaluatorForExpressionType
    {
        readonly IRegistersExpressionEvaluator registry;
        readonly IServiceProvider resolver;

        /// <summary>
        /// Gets the evaluator which is appropriate for use with the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns>The evaluator.</returns>
        /// <param name="expressionType">Expression type.</param>
        public IEvaluatesExpression GetEvaluator(string expressionType)
        {
            var implType = registry.GetEvaluatorType(expressionType);
            return (IEvaluatesExpression) resolver.GetRequiredService(implType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluatorTypeResolver"/> class.
        /// </summary>
        /// <param name="registry">Registry.</param>
        /// <param name="resolver">Resolver.</param>
        public EvaluatorTypeResolver(IRegistersExpressionEvaluator registry, IServiceProvider resolver)
        {
            this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}
