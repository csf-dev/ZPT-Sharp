using System.Threading.Tasks;

namespace ZptSharp
{
    /// <summary>
    /// An object which can get a markup string, which may be inserted directly into a template
    /// document without any escaping or encoding.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Object which implement this interface will be written by TAL <c>content</c>/<c>replace</c>
    /// attributes as if they used the <c>structure</c> keyword even if the keyword is not present.
    /// Obviously, it is crucial for the developer to ensure that the markup returned by this object
    /// is safe.  Primarily this means that either the markup contains no user-generated content, or
    /// at least that it has been properly sanitised.
    /// </para>
    /// </remarks>
    public interface IGetsStructuredMarkup
    {
        /// <summary>
        /// Gets the structured markup from the current instance.
        /// </summary>
        /// <returns>A task which returns a structured markup string (an HTML or XML fragment).</returns>
        Task<string> GetMarkupAsync();
    }
}