using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;

namespace ZptSharp.Expressions.PathExpressions
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
        /// <summary>
        /// Gets the rendering configuration.
        /// </summary>
        protected readonly RenderingConfig config;

        /// <summary>
        /// Gets the builtin contexts provider factory.
        /// </summary>
        protected readonly IGetsBuiltinContextsProvider builtinContextsProviderFactory;

        /// <summary>
        /// Gets the document reader/writer.
        /// </summary>
        protected readonly Dom.IReadsAndWritesDocument readerWriter;

        /// <summary>
        /// Gets the METAL document adapter factory.
        /// </summary>
        protected readonly Metal.IGetsMetalDocumentAdapter adapterFactory;

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
        protected virtual IGetsValueFromObject BuildChainOfResponsibility()
        {
            // Note that this is written "upside down".  The services are actually executed in
            // last-to-first order.  Each service wraps (and thus gets a chance to execute
            // before) the one which precedes it.
            var service = GetFailureService();
            service = GetReflectionValueLink(service);
            service = GetEnumerableValueLink(service);
            service = GetDynamicValueLink(service);
            service = GetIntegerKeyedDictionaryValueLink(service);
            service = GetStringKeyedDictionaryValueLink(service);
            service = GetNamedValueLink(service);
            service = GetTemplateDirectoryValueLink(service);
            service = GetContextWrappingDecorator(service);
            return service;
        }

        /// <summary>
        /// Gets the one service which is not a decorator or chain of responsibility 'link' class.
        /// This service returns hard-coded failure.  If it is executed then it means that every
        /// other service in the chain has failed to return a conclusive result.
        /// </summary>
        /// <returns>The failure service.</returns>
        protected virtual IGetsValueFromObject GetFailureService() => new FailureValueProvider();

        /// <summary>
        /// Gets a decorator which pre-processes the 2nd parameter to
        /// <see cref="IGetsValueFromObject.TryGetValueAsync(string, object, CancellationToken)"/>.
        /// If that parameter is an <see cref="ExpressionContext"/> then it is substituted with
        /// <see cref="NamedTalesValueForExpressionContextAdapter"/> wrapping the original context.
        /// </summary>
        /// <returns>The context-wrapping decorator.</returns>
        /// <param name="service">Service.</param>
        protected virtual IGetsValueFromObject GetContextWrappingDecorator(IGetsValueFromObject service)
            => new ExpressionContextWrappingDecorator(config, builtinContextsProviderFactory, service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes named values.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetNamedValueLink(IGetsValueFromObject service)
            => new NamedTalesValueProvider(service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes string-keyed dictionaries.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetStringKeyedDictionaryValueLink(IGetsValueFromObject service)
            => new StringKeyedDictionaryValueProvider(service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes integer-keyed dictionaries.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetIntegerKeyedDictionaryValueLink(IGetsValueFromObject service)
            => new IntegerKeyedDictionaryValueProvider(service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes dynamic objects.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetDynamicValueLink(IGetsValueFromObject service)
            => new DynamicObjectValueProvider(service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes enumerable objects.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetEnumerableValueLink(IGetsValueFromObject service)
            => new EnumerableValueProvider(service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes objects using reflection.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetReflectionValueLink(IGetsValueFromObject service)
            => new ReflectionObjectValueProvider(service);

        /// <summary>
        /// Gets a chain-of-responsibility link which processes template directory objects.
        /// </summary>
        /// <returns>The chain of responsibility link.</returns>
        /// <param name="service">The service to be wrapped by this additional link.</param>
        protected virtual IGetsValueFromObject GetTemplateDirectoryValueLink(IGetsValueFromObject service)
            => new TemplateDirectoryValueProvider(service, readerWriter, config, adapterFactory);

        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratorBasedObjectValueProvider"/> class.
        /// </summary>
        /// <param name="config">Config.</param>
        /// <param name="builtinContextsProviderFactory">Builtin contexts provider factory.</param>
        /// <param name="readerWriter">A document reader/writer.</param>
        /// <param name="adapterFactory">A METAL document adapter factory.</param>
        public DecoratorBasedObjectValueProvider(RenderingConfig config,
                                                 IGetsBuiltinContextsProvider builtinContextsProviderFactory,
                                                 Dom.IReadsAndWritesDocument readerWriter,
                                                 Metal.IGetsMetalDocumentAdapter adapterFactory)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.builtinContextsProviderFactory = builtinContextsProviderFactory ?? throw new ArgumentNullException(nameof(builtinContextsProviderFactory));
            this.readerWriter = readerWriter ?? throw new ArgumentNullException(nameof(readerWriter));
            this.adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
        }
    }
}
