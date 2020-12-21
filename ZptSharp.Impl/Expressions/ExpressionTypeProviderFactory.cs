namespace ZptSharp.Expressions
{
    /// <summary>
    /// Default implementation of <see cref="IGetsExpressionTypeProvider"/>.
    /// </summary>
    public class ExpressionTypeProviderFactory : IGetsExpressionTypeProvider
    {
        readonly Config.RenderingConfig config;

        /// <summary>
        /// Gets an implementation of <see cref="IGetsExpressionType"/>.
        /// </summary>
        /// <returns>The expression type provider.</returns>
        public IGetsExpressionType GetTypeProvider()
            => new ConfiguredDefaultExpressionTypeDecorator(new PrefixExpressionTypeProvider(), config);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTypeProviderFactory"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        public ExpressionTypeProviderFactory(Config.RenderingConfig config)
        {
            this.config = config ?? throw new System.ArgumentNullException(nameof(config));
        }
    }
}
