using System;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Expressions;
using System.IO;
using ZptSharp.Dom;
using ZptSharp.Config;
using ZptSharp.Rendering;
using ZptSharp.Metal;

namespace ZptSharp.Expressions.PathExpressions
{
    /// <summary>
    /// A chain of responsibility class which get values from an objects of type
    /// <see cref="TemplateDirectory"/>.  If the input object is a template directory then
    /// the output will either be a ZPT document adapter created via <see cref="IGetsMetalDocumentAdapter"/>,
    /// or it will be another template directory (representing directory traversal) or an exception will
    /// be raised.
    /// </summary>
    public class TemplateDirectoryValueProvider : IGetsValueFromObject
    {
        readonly IGetsValueFromObject wrapped;
        readonly IReadsAndWritesDocument readerWriter;
        readonly RenderingConfig config;
        readonly IGetsMetalDocumentAdapter adapterFactory;

        /// <summary>
        /// Attempts to get a value for a named reference, from the specified object.
        /// </summary>
        /// <returns>An object indicating whether a value was successfully retrieved or not, along with the retrieved value (if applicable).</returns>
        /// <param name="name">The value name.</param>
        /// <param name="object">The object from which to retrieve the value.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        public async Task<GetValueResult> TryGetValueAsync(string name, object @object, CancellationToken cancellationToken = default)
        {
            if (!(@object is TemplateDirectory templateDirectory))
                return await wrapped.TryGetValueAsync(name, @object, cancellationToken);

            var path = Path.Combine(templateDirectory.Path, name);

            if (File.Exists(path))
                return GetValueResult.For(await GetMetalAdapterAsync(path, cancellationToken));

            if (Directory.Exists(path))
                return GetValueResult.For(new TemplateDirectory(path));

            string message = String.Format(Resources.ExceptionMessage.DocumentNotFound, path);
            throw new FileNotFoundException(message);
        }

        /// <summary>
        /// Gets an instance of <see cref="IGetsNamedTalesValue"/> to wrap the ZPT
        /// document exposed by the specified <paramref name="path"/>.
        /// The returned object will be an adapter created via <see cref="IGetsMetalDocumentAdapter"/>,
        /// enabling access to that document's METAL macros.
        /// </summary>
        /// <returns>The METAL document adapter.</returns>
        /// <param name="path">Path.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        async Task<IGetsNamedTalesValue> GetMetalAdapterAsync(string path, CancellationToken cancellationToken)
        {
            var doc = await GetDocumentAsync(path, cancellationToken);
            return adapterFactory.GetMetalDocumentAdapter(doc);
        }

        async Task<IDocument> GetDocumentAsync(string path, CancellationToken cancellationToken)
        {
            var source = new FileSourceInfo(path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                return await readerWriter.GetDocumentAsync(stream, config, source, cancellationToken);
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DynamicObjectValueProvider"/> class.
        /// </summary>
        /// <param name="wrapped">Wrapped.</param>
        /// <param name="readerWriter">A document reader/writer.</param>
        /// <param name="config">The rendering configuration.</param>
        /// <param name="adapterFactory">A METAL document adapter factory.</param>
        public TemplateDirectoryValueProvider(IGetsValueFromObject wrapped,
                                              IReadsAndWritesDocument readerWriter,
                                              RenderingConfig config,
                                              IGetsMetalDocumentAdapter adapterFactory)
        {
            this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            this.readerWriter = readerWriter ?? throw new ArgumentNullException(nameof(readerWriter));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.adapterFactory = adapterFactory ?? throw new ArgumentNullException(nameof(adapterFactory));
        }
    }
}
