using System;
using ZptSharp.Config;
using ZptSharp.Dom;
using ZptSharp.Metal;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// Special case of <see cref="DecoratorBasedObjectValueProvider"/> which will only get values from
    /// an expression context by use of local variables, and not any other kind of variable.
    /// </summary>
    public class LocalVariableOnlyDecoratorBasedObjectValueProvider : DecoratorBasedObjectValueProvider, IGetsValueFromObjectWithLocalVariablesOnly
    {
        /// <summary>
        /// Gets a decorator which pre-processes the 2nd parameter to
        /// <see cref="M:ZptSharp.Expressions.PathExpressions.IGetsValueFromObject.TryGetValueAsync(System.String,System.Object,System.Threading.CancellationToken)"/>.
        /// If that parameter is an <see cref="T:ZptSharp.Expressions.ExpressionContext"/> then it is substituted with
        /// <see cref="T:ZptSharp.Expressions.NamedTalesValueForExpressionContextAdapter"/> wrapping the original context.
        /// </summary>
        /// <returns>The context-wrapping decorator.</returns>
        /// <param name="service">Service.</param>
        protected override IGetsValueFromObject GetContextWrappingDecorator(IGetsValueFromObject service)
            => new LocalVariableOnlyExpressionContextWrappingDecorator(service);

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="LocalVariableOnlyDecoratorBasedObjectValueProvider"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        /// <param name="readerWriter">Reader writer.</param>
        /// <param name="adapterFactory">Adapter factory.</param>
        public LocalVariableOnlyDecoratorBasedObjectValueProvider(RenderingConfig config,
                                                                  IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                                  IReadsAndWritesDocument readerWriter,
                                                                  IGetsMetalDocumentAdapter adapterFactory)
            : base(config, builtinContextsProviderFactory, readerWriter, adapterFactory) { }
    }
}
