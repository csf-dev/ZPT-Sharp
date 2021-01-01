using System.Threading;
using System.Threading.Tasks;

namespace ZptSharp
{
    /// <summary>
    /// An object which can load a model object from a file path.
    /// </summary>
    public interface ILoadsModel
    {
        /// <summary>
        /// Loads and returns the model.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="token">An optional cancallation token.</param>
        /// <returns>The model object.</returns>
        Task<object> LoadModelAsync(string path, CancellationToken token = default);
    }
}