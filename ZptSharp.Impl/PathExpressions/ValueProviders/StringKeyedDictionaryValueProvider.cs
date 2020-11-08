using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    /// <summary>
    /// A chain of responsibility class which get values from any objects which implement any form of
    /// <c>IDictionary&lt;string,T&gt;</c>.  If they implement a dictionary of string and 'any type' then
    /// we may read values from them based upon their string keys.
    /// </summary>
    public class StringKeyedDictionaryValueProvider : IGetsValueFromObject
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
        /// If either the object does not implement any dictionary interfaces keyed by string, or
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
            var dictionary = adapterFactory.GetDictionary<string>(@object);

            if (dictionary != null && dictionary.ContainsKey(name))
                return Task.FromResult(GetValueResult.For(dictionary[name]));

            // A special case: If we did not find a named value but the input object is
            // an ExpandoObject then we know for certain that we will never get a result,
            // so we can conclusively return failure here.
            if (@object is System.Dynamic.ExpandoObject)
                return Task.FromResult(GetValueResult.Failure);

            return wrapped.TryGetValueAsync(name, @object, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="StringKeyedDictionaryValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        public StringKeyedDictionaryValueProvider(IGetsValueFromObject wrapped)
        {
            adapterFactory = new KeyedDictionaryAdapterFactory();
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
