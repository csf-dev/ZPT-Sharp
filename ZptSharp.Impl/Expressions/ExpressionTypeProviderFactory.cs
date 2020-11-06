using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// Default implementation of <see cref="IGetsExpressionTypeProvider"/>.
    /// </summary>
    public class ExpressionTypeProviderFactory : IGetsExpressionTypeProvider
    {
        /// <summary>
        /// Gets an implementation of <see cref="IGetsExpressionType"/>.
        /// </summary>
        /// <returns>The expression type provider.</returns>
        public IGetsExpressionType GetTypeProvider()
            => new DefaultPathExpressionDecorator(new PrefixExpressionTypeProvider());
    }
}
