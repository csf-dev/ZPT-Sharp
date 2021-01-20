using System;
using ZptSharp.Config;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// Decorator for the <see cref="IGetsExpressionType"/> service.  If the wrapped service
    /// does not produce a non-null result, then this decorator returns the
    /// <see cref="RenderingConfig.DefaultExpressionType"/>.
    /// </summary>
    public class ConfiguredDefaultExpressionTypeDecorator : IGetsExpressionType
    {
        readonly IGetsExpressionType wrapped;
        readonly RenderingConfig config;

        /// <summary>
        /// Gets the expression type for the specified <paramref name="expression"/>.
        /// </summary>
        /// <returns>The expression type.</returns>
        /// <param name="expression">An expression.</param>
        public string GetExpressionType(string expression)
            => wrapped.GetExpressionType(expression) ?? config.DefaultExpressionType;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ConfiguredDefaultExpressionTypeDecorator"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="config">Config.</param>
        public ConfiguredDefaultExpressionTypeDecorator(IGetsExpressionType wrapped, RenderingConfig config)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }
    }
}
