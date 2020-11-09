using System;
using System.Collections.Generic;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which is used to register implementations of <see cref="IEvaluatesExpression"/> to be used
    /// for specific expression types.
    /// </summary>
    public interface IRegistersExpressionEvaluator
    {
        /// <summary>
        /// Registers the evaluator type for the specified expression type.
        /// </summary>
        /// <param name="evaluatorType">The evaluator implementation type.</param>
        /// <param name="expressionType">The associated expression type.</param>
        void RegisterEvaluatorType(Type evaluatorType, string expressionType);

        /// <summary>
        /// Gets a value that indicates whether there is an evaluator type registered for
        /// the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns><c>true</c>, if an evaluator type is registered, <c>false</c> otherwise.</returns>
        /// <param name="expressionType">Expression type.</param>
        bool IsRegistered(string expressionType);

        /// <summary>
        /// Removes the registered evaluator type for the specified expression type, if it exists.
        /// </summary>
        /// <returns><c>true</c> if an evaluator type was registered and has now been unregistered; <c>false</c> if the expression type did not previously have an evaluator type registered.</returns>
        /// <param name="expressionType">Expression type.</param>
        bool Unregister(string expressionType);

        /// <summary>
        /// Gets a collection of all of the registered expression types.
        /// </summary>
        /// <returns>The all registered types.</returns>
        IReadOnlyCollection<string> GetRegisteredExpressionTypes();

        /// <summary>
        /// Gets the evaluator type for the specified <paramref name="expressionType"/>.
        /// </summary>
        /// <returns>The evaluator type.</returns>
        /// <param name="expressionType">Expression type.</param>
        Type GetEvaluatorType(string expressionType);
    }
}
