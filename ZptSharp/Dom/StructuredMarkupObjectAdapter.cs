using System.Threading.Tasks;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IGetsStructuredMarkup"/> which wraps any <c>object</c>,
    /// exposing its <c>Object.ToString()</c> method via <see cref="GetMarkupAsync"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Please read the documentation for <see cref="IGetsStructuredMarkup"/> relating to security
    /// concerns and markup sanitisation before using this object.
    /// </para>
    /// </remarks>
    public class StructuredMarkupObjectAdapter : IGetsStructuredMarkup
    {
        readonly object obj;

        /// <summary>
        /// Gets the structured markup from the current instance.
        /// </summary>
        /// <returns>A task which returns a structured markup string (an HTML or XML fragment).</returns>
        public Task<string> GetMarkupAsync() => Task.FromResult<string>(obj?.ToString());

        /// <summary>
        /// Initializes a new instance of <see cref="StructuredMarkupObjectAdapter"/>.
        /// </summary>
        /// <param name="obj">The object instance to wrap.</param>
        public StructuredMarkupObjectAdapter(object obj)
        {
            this.obj = obj;
        }
    }
}