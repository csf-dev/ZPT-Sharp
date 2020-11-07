using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;

namespace ZptSharp.PathExpressions
{
    /// <summary>
    /// An object which traverses a specified object to get a value of a specified name.
    /// </summary>
    public interface IGetsValueFromObject
    {
        /// <summary>
        /// Attempts to get a value for a named reference, from the specified object.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default);
    }
}
