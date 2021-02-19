using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of <see cref="IRegistersExpressionEvaluator"/> which uses a
    /// in-memory dictionary to maintain registrations of evaluator types.
    /// </summary>
    public class EvaluatorTypeRegistry : IRegistersExpressionEvaluator
    {
        readonly Hosting.EnvironmentRegistry registry;

        /// <summary>
        /// Gets the evaluator type for the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns>The evaluator type.</returns>
        /// <param name="expressionType">Expression type.</param>
        public Type GetEvaluatorType(string expressionType)
        {
            if (expressionType == null)
                throw new ArgumentNullException(nameof(expressionType));
            AssertIsRegistered(expressionType);

            return registry.ExpresionEvaluatorTypes[expressionType];
        }

        /// <summary>
        /// Gets a value that indicates whether there is an evaluator type registered for
        /// the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns><c>true</c>, if an evaluator type is registered, <c>false</c> otherwise.</returns>
        /// <param name="expressionType">Expression type.</param>
        public bool IsRegistered(string expressionType)
        {
            if (expressionType == null)
                throw new ArgumentNullException(nameof(expressionType));
            return registry.ExpresionEvaluatorTypes.ContainsKey(expressionType);
        }

        /// <summary>
        /// Gets a collection of all of the registered expression types.
        /// </summary>
        /// <returns>The all registered types.</returns>
        public IReadOnlyCollection<string> GetRegisteredExpressionTypes()
            => registry.ExpresionEvaluatorTypes.Keys.ToArray();

        void AssertIsRegistered(string expressionType)
        {
            if (registry.ExpresionEvaluatorTypes.ContainsKey(expressionType)) return;

            string message = String.Format(Resources.ExceptionMessage.NoEvaluatorForExpressionType,
                                           expressionType,
                                           $"{nameof(IRegistersExpressionEvaluator)}.{nameof(Hosting.EnvironmentRegistry)}");
            throw new NoEvaluatorForExpressionTypeException(message);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="EvaluatorTypeRegistry"/>.
        /// </summary>
        /// <param name="registry">The environment registry.</param>
        public EvaluatorTypeRegistry(Hosting.EnvironmentRegistry registry)
        {
            this.registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }
    }
}
