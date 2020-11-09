using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions.ValueProviders
{
    /// <summary>
    /// A chain of responsibility class which detects when the object from
    /// which to get a value is an implementation of <see cref="IGetsNamedTalesValue"/>.
    /// If it is, then its built-in functionality is used to get the value.
    /// </summary>
    public class NamedTalesValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;

        /// <summary>
        /// Attempts to get a value for a named reference, from the specified object.
        /// If that object is an implementation of <see cref="IGetsNamedTalesValue"/> then
        /// <see cref="IGetsNamedTalesValue.TryGetValueAsync(string, CancellationToken)"/> is
        /// used to get the result. Otherwise, the call is delegated to the wrapped service.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            if(@object is IGetsNamedTalesValue valueProvider)
                return valueProvider.TryGetValueAsync(name, cancellationToken);

            return wrapped.TryGetValueAsync(name, @object, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="NamedTalesValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        public NamedTalesValueProvider(IGetsValueFromObject wrapped)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
        }
    }
}
