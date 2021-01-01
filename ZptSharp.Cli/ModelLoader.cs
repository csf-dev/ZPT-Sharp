using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace ZptSharp
{
    /// <summary>
    /// Implementation of <see cref="ILoadsModel" /> which loads a model from a JSON source file.
    /// </summary>
    public class ModelLoader : ILoadsModel
    {
        /// <summary>
        /// Loads and returns the model.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="token">An optional cancallation token.</param>
        /// <returns>The model object.</returns>
        public async Task<object> LoadModelAsync(string path, CancellationToken token = default)
        {
            using(var streamReader = new StreamReader(path))
            using(var jsonReader = new JsonTextReader(streamReader))
                return await JObject.LoadAsync(jsonReader, token);
        }
    }
}