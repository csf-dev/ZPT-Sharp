using System;
namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which may be used as a factory for instances of <see cref="IGetsExpressionType"/>.
    /// </summary>
    public interface IGetsExpressionTypeProvider
    {
        /// <summary>
        /// Gets an implementation of <see cref="IGetsExpressionType"/>.
        /// </summary>
        /// <returns>The expression type provider.</returns>
        IGetsExpressionType GetTypeProvider();
    }
}
