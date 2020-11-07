using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// A decorator for <see cref="IGetsExpressionType"/> which (if the wrapped service
    /// returns <see langword="null"/>) defaults the expression type to <see cref="WellKnownExpressionType.Path"/>.
    /// </summary>
    public class DefaultPathExpressionDecorator : IGetsExpressionType
    {
        readonly IGetsExpressionType wrapped;

        /// <summary>
        /// Gets the expression type for the specified <paramref name="expression"/>.
        /// </summary>
        /// <returns>The expression type.</returns>
        /// <param name="expression">An expression.</param>
        public string GetExpressionType(string expression)
            => wrapped.GetExpressionType(expression) ?? WellKnownExpressionType.Path;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultPathExpressionDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">The wrapped implementation.</param>
        public DefaultPathExpressionDecorator(IGetsExpressionType wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
