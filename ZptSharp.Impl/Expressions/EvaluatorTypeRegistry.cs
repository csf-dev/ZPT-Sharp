using System;
using System.Collections.Generic;
using System.Linq;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Implementation of both <see cref="IGetsEvaluatorForExpressionType"/> and
    /// <see cref="IRegistersExpressionEvaluator"/> which uses a <see cref="IServiceProvider"/>
    /// and in-memory dictionary to maintain registrations of evaluator types and resolve them.
    /// </summary>
    public class EvaluatorTypeRegistry : IGetsEvaluatorForExpressionType, IRegistersExpressionEvaluator
    {
        readonly IDictionary<string, Type> registry = new Dictionary<string,Type>();
        readonly IServiceProvider resolver;

        /// <summary>
        /// Gets the evaluator which is appropriate for use with the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns>The evaluator.</returns>
        /// <param name="expressionType">Expression type.</param>
        public IEvaluatesExpression GetEvaluator(string expressionType)
        {
            if (expressionType == null)
                throw new ArgumentNullException(nameof(expressionType));
            AssertIsRegistered(expressionType);

            var type = registry[expressionType];
            return (IEvaluatesExpression) resolver.GetService(type);
        }

        /// <summary>
        /// Registers the evaluator type for the specified expression type.
        /// </summary>
        /// <param name="evaluatorType">The evaluator implementation type.</param>
        /// <param name="expressionType">The associated expression type.</param>
        public void RegisterEvaluatorType(Type evaluatorType, string expressionType)
        {
            if (evaluatorType == null)
                throw new ArgumentNullException(nameof(evaluatorType));
            if (String.IsNullOrEmpty(expressionType))
                throw new ArgumentException(Resources.ExceptionMessage.ExpressionTypeMustNotBeNullOrEmpty, nameof(expressionType));
            AssertIsNotRegistered(expressionType);
            AssertIsCorrectImplementationType(evaluatorType);

            registry.Add(expressionType, evaluatorType);
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
            return registry.ContainsKey(expressionType);
        }

        /// <summary>
        /// Removes the registered evaluator type for the specified expression type, if it exists.
        /// </summary>
        /// <returns><c>true</c> if an evaluator type was registered and has now been unregistered; <c>false</c> if the expression type did not previously have an evaluator type registered.</returns>
        /// <param name="expressionType">Expression type.</param>
        public bool Unregister(string expressionType)
        {
            if (expressionType == null)
                throw new ArgumentNullException(nameof(expressionType));
            return registry.Remove(expressionType);
        }

        /// <summary>
        /// Gets a collection of all of the registered expression types.
        /// </summary>
        /// <returns>The all registered types.</returns>
        public IReadOnlyCollection<string> GetRegisteredExpressionTypes()
            => registry.Keys.ToArray();

        void AssertIsRegistered(string expressionType)
        {
            if (registry.ContainsKey(expressionType)) return;

            string message = String.Format(Resources.ExceptionMessage.NoEvaluatorForExpressionType,
                                           expressionType,
                                           $"{nameof(IRegistersExpressionEvaluator)}.{nameof(IRegistersExpressionEvaluator.RegisterEvaluatorType)}");
            throw new NoEvaluatorForExpressionTypeException(message);
        }

        void AssertIsNotRegistered(string expressionType)
        {
            if (!registry.ContainsKey(expressionType)) return;

            string message = String.Format(Resources.ExceptionMessage.EvaluatorAlreadyRegistered,
                                           expressionType,
                                           $"{nameof(IRegistersExpressionEvaluator)}.{nameof(IRegistersExpressionEvaluator.Unregister)}");
            throw new ArgumentException(message, nameof(expressionType));
        }

        void AssertIsCorrectImplementationType(Type evaluatorType)
        {
            if (typeof(IEvaluatesExpression).IsAssignableFrom(evaluatorType)) return;

            string message = String.Format(Resources.ExceptionMessage.EvaluatorTypeMustImplementInterface,
                                           typeof(IEvaluatesExpression).FullName,
                                           evaluatorType.FullName);
            throw new ArgumentException(message, nameof(evaluatorType));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluatorTypeRegistry"/> class.
        /// </summary>
        /// <param name="resolver">Resolver.</param>
        public EvaluatorTypeRegistry(IServiceProvider resolver)
        {
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }
    }
}
