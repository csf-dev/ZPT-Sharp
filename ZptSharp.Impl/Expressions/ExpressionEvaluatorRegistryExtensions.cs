using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// Extension methods for <see cref="IRegistersExpressionEvaluator"/>
    /// </summary>
    public static class ExpressionEvaluatorRegistryExtensions
    {
        /// <summary>
        /// Registers the evaluator type for the specified expression type.
        /// </summary>
        /// <param name="registry">The evaluator type registry.</param>
        /// <param name="expressionType">The associated expression type.</param>
        /// <typeparam name="T">The evaluator implementation type.</typeparam>
        public static void RegisterEvaluatorType<T>(this IRegistersExpressionEvaluator registry,
                                                    string expressionType)
            where T : class,IEvaluatesExpression
        {
            if (registry == null)
                throw new ArgumentNullException(nameof(registry));

            registry.RegisterEvaluatorType(typeof(T), expressionType);
        }
    }
}
