using System;
using ZptSharp.Config;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Factory for instances of <see cref="IWalksAndEvaluatesPathExpression"/>.  Specifically this
    /// deals with the creation of an appropriate instance of <see cref="DecoratorBasedObjectValueProvider"/>,
    /// based on the provided <see cref="RootScopeLimitation"/>.
    /// </summary>
    public class PathWalkingExpressionEvaluatorFactory : IGetsPathWalkingExpressionEvaluator
    {
        readonly RenderingConfig config;
        readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;
        readonly Dom.IReadsAndWritesDocument readerWriter;
        readonly Metal.IGetsMetalDocumentAdapter adapterFactory;
        readonly IGetsValueFromReflection reflectionValueProvider;

        /// <summary>
        /// Gets an instance of <see cref="IWalksAndEvaluatesPathExpression"/> suitable for use with the
        /// specified root scope <paramref name="limitation"/>.
        /// </summary>
        /// <returns>The evaluator.</returns>
        /// <param name="limitation">Limitation.</param>
        public IWalksAndEvaluatesPathExpression GetEvaluator(RootScopeLimitation limitation)
        {
            var valueProvider = new DecoratorBasedObjectValueProvider(config,
                                                                      builtinContextsProviderFactory,
                                                                      readerWriter,
                                                                      adapterFactory,
                                                                      limitation,
                                                                      reflectionValueProvider);
            return new PathWalkingExpressionEvaluator(valueProvider);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="PathWalkingExpressionEvaluatorFactory"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        /// <param name="readerWriter">Reader writer.</param>
        /// <param name="adapterFactory">Adapter factory.</param>
        /// <param name="reflectionValueProvider">The reflection value provider.</param>
        public PathWalkingExpressionEvaluatorFactory(RenderingConfig config,
                                                     IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                     Dom.IReadsAndWritesDocument readerWriter,
                                                     Metal.IGetsMetalDocumentAdapter adapterFactory,
                                                     IGetsValueFromReflection reflectionValueProvider)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
            this.readerWriter = readerWriter ?? throw new ArgumentNullException(nameof(readerWriter));
            this.adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
            this.reflectionValueProvider = reflectionValueProvider ?? throw new ArgumentNullException(nameof(reflectionValueProvider));
        }
    }
}
