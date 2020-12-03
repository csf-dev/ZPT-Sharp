using System;
using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A chain of responsibility class which get values from any objects which implement any form of
    /// <c>IDictionary&lt;int,T&gt;</c>.  If they implement a dictionary of integer and 'any type' then
    /// we may read values from them based upon their numeric keys.
    /// </summary>
    public class IntegerKeyedDictionaryValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;
        readonly KeyedDictionaryAdapterFactory adapterFactory;

        /// <summary>
        /// <para>
        /// Attempts to get a value for a named reference, from the specified object.
        /// </para>
        /// <para>
        /// This will read a named value from that object if it implements a suitable
        /// generic dictionary interface and contains a key matching the <paramref name="name"/>.
        /// If either the object does not implement any dictionary interfaces keyed by integer, or
        /// does not contain a key matching the name, then the call is delegated to the wrapped
        /// service.
        /// </para>
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            var dictionary = adapterFactory.GetDictionary<int>(@object);

            if (dictionary != null && Int32.TryParse(name, out var index) && dictionary.ContainsKey(index))
                return Task.FromResult(GetValueResult.For(dictionary[index]));

            return wrapped.TryGetValueAsync(name, @object, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="StringKeyedDictionaryValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        public IntegerKeyedDictionaryValueProvider(IGetsValueFromObject wrapped)
        {
            adapterFactory = new KeyedDictionaryAdapterFactory();
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
