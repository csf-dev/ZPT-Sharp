using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An implementation of <see cref="IGetsValueFromObject"/> which works for types which implement
    /// <see cref="IEnumerable"/>.  This allows accessing their items by zero-based numeric index.
    /// </summary>
    public class EnumerableValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;

        /// <summary>
        /// Attempts to get a value for a named reference, from the specified object.
        /// If the object implements <see cref="IEnumerable"/> and the <paramref name="name"/> is a valid
        /// integer then an item will be returned from the zero-based index derived from the name.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            if (!(@object is IEnumerable enumerable) || !Int32.TryParse(name, out var index))
                return wrapped.TryGetValueAsync(name, @object, cancellationToken);

            // Pointless trying anything further if the index is less than zero
            if (index < 0) return Task.FromResult(GetValueResult.Failure);

            // An optimisation in case the enumerable implements IList
            if(enumerable is System.Collections.IList list)
            {
                return Task.FromResult((index < list.Count) ? GetValueResult.For(list[index]) : GetValueResult.Failure);
            }

            var result = enumerable.Cast<object>().Skip(index).Take(1);
            return Task.FromResult(result.Any() ? GetValueResult.For(result.Single()) : GetValueResult.Failure);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumerableValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        public EnumerableValueProvider(IGetsValueFromObject wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
