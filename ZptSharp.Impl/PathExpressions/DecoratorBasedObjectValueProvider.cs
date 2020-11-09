using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Expressions;
using ZptSharp.PathExpressions.ValueProviders;

namespace ZptSharp.PathExpressions
{
    /// <summary>
    /// <para>
    /// An implementation of <see cref="IGetsValueFromObject"/> which builds and delegates to
    /// a service using a combination of the decorator &amp; chain of responsibility patterns.
    /// </para>
    /// <para>
    /// That service ultimately implements the same interface as this class but provides logic
    /// declared in many other classes.
    /// </para>
    /// </summary>
    public class DecoratorBasedObjectValueProvider : IGetsValueFromObject
    {
        readonly RenderingConfig config;
        readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;

        /// <summary>
        /// Attempts to get a value for a named reference, from the specified object.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name,
                                                     object @object,
                                                     CancellationToken cancellationToken = default)
        {
            var service = BuildChainOfResponsibility();
            return service.TryGetValueAsync(name, @object, cancellationToken);
        }

        /// <summary>
        /// <para>
        /// Builds the service which shall be used, using chain of responsibility &amp; decorator patterns.
        /// Most classes used are chain of responsibility links, but a limited number of decorators are used to
        /// pre-process/substitute parameters before they go into other classes.
        /// </para>
        /// <para>
        /// See https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern &amp;
        /// https://en.wikipedia.org/wiki/Decorator_pattern
        /// </para>
        /// </summary>
        /// <returns>The chain of responsibility.</returns>
        IGetsValueFromObject BuildChainOfResponsibility()
        {
            // Note that this is written "upside down".  The services are executed in
            // last-first order, from bottom-to-top.  Each service wraps the one which precedes it.

            var service = GetFailureService();

            service = GetDynamicValueLink(service);
            service = GetIntegerKeyedDictionaryValueLink(service);
            service = GetStringKeyedDictionaryValueLink(service);
            service = GetNamedValueLink(service);
            service = GetContextWrappingDecorator(service);

            return service;
        }

        /// <summary>
        /// Gets the one service which is not a decorator or chain of responsibility 'link' class.
        /// This service returns hard-coded failure.  If it is executed then it means that every
        /// other service in the chain has failed to return a conclusive result.
        /// </summary>
        /// <returns>The failure service.</returns>
        IGetsValueFromObject GetFailureService() => new FailureValueProvider();

        IGetsValueFromObject GetContextWrappingDecorator(IGetsValueFromObject service)
            => new ExpressionContextWrappingDecorator(config, builtinContextsProviderFactory, service);

        IGetsValueFromObject GetNamedValueLink(IGetsValueFromObject service)
            => new NamedTalesValueProvider(service);

        IGetsValueFromObject GetStringKeyedDictionaryValueLink(IGetsValueFromObject service)
            => new StringKeyedDictionaryValueProvider(service);

        IGetsValueFromObject GetIntegerKeyedDictionaryValueLink(IGetsValueFromObject service)
            => new IntegerKeyedDictionaryValueProvider(service);

        IGetsValueFromObject GetDynamicValueLink(IGetsValueFromObject service)
            => new DynamicObjectValueProvider(service);

        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratorBasedObjectValueProvider"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        public DecoratorBasedObjectValueProvider(RenderingConfig config,
                                                 IGetsBuiltinContextsProvider builtinContextsProviderFactory)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
        }
    }
}
