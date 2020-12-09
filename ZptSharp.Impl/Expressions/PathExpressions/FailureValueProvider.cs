using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// An implementation of <see cref="IGetsValueFromObject"/> which returns a failure result.
    /// This should always be the 'last' link in a chain of responsibility/decorator stack.  If
    /// the execution reaches this class then getting a value has failed.
    /// </summary>
    public class FailureValueProvider : IGetsValueFromObject
    {
        /// <summary>
        /// Returns a failure result, indicating that getting a value has not been possible.
        /// </summary>
        /// <returns>An object indicating failure to get a named value from the object.</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
            => Task.FromResult(GetValueResult.Failure);
    }
}
