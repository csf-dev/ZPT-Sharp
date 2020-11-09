using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp.Expressions
{
    /// <summary>
    /// An object which provides its own logic for getting named values when resolving a TALES expression.
    /// </summary>
    /// <remarks>
    /// <para>
    /// General objects are traversed using common rules defined within ZPT.  However, objects implementing this
    /// interface declare that they provide their own specific logic for traversal.
    /// The <see cref="TryGetValueAsync(string, CancellationToken)"/> method will be used instead of the usual
    /// traversal rules in order to get the value of a named reference, relative to the current object.
    /// </para>
    /// </remarks>
    public interface IGetsNamedTalesValue
    {
        /// <summary>
        /// Attempts to get a value for a named reference, relative to the current instance.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The name of the value to retrieve.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        Task<GetValueResult> TryGetValueAsync(string name, CancellationToken cancellationToken = default);
    }
}
