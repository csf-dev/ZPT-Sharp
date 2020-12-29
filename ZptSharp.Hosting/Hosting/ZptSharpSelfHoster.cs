using System;
using Microsoft.Extensions.DependencyInjection;
using ZptSharp.Rendering;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// An implementation of <see cref="IHostsZptSharp" /> which wraps an
    /// <see cref="IServiceProvider" /> and uses that to provide the entry-points.
    /// </summary>
    public class ZptSharpSelfHoster : IHostsZptSharp
    {
        bool disposedValue;

        /// <summary>
        /// Gets the underlying service provider from which the current hosting environment is built.
        /// </summary>
        /// <value>The service provider.</value>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets a service which renders ZPT template files from disk paths.
        /// </summary>
        /// <value>The file renderer.</value>
        public IRendersZptFile FileRenderer => ServiceProvider.GetRequiredService<IRendersZptFile>();

        /// <summary>
        /// <para>
        /// Gets a service which may be used to create an instance of <see cref="IRendersZptDocument" />,
        /// suitable for use with a given file path.  The file path will be used to detect the type of
        /// document.
        /// </para>
        /// <para>
        /// Note that if you wish to render a template from disk, then it is easier to use
        /// <see cref="FileRenderer" /> instead.  This service is appropriate when the file path
        /// might not exist, but a stream is being provided for the template document anyway.
        /// </para>
        /// </summary>
        /// <value>The document renderer factory for file paths.</value>
        public IGetsZptDocumentRendererForFilePath DocumentRendererForPathFactory
            => ServiceProvider.GetRequiredService<IGetsZptDocumentRendererForFilePath>();

        /// <summary>
        /// Gets a service which copies a <see cref="System.IO.Stream" /> to a
        /// <see cref="System.IO.TextWriter" />.
        /// </summary>
        /// <value>The stream-copying service.</value>
        public IWritesStreamToTextWriter StreamCopier => ServiceProvider.GetRequiredService<IWritesStreamToTextWriter>();

        /// <summary>
        /// Dispose of the current instance and release its resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the current instance.
        /// </summary>
        /// <param name="disposing">If <c>true</c> then this disposal is explicit.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && ServiceProvider is IDisposable disposableProvider)
                {
                    disposableProvider.Dispose();
                }

                disposedValue = true;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="ZptSharpSelfHoster" />.
        /// </summary>
        /// <param name="serviceProvider">A service provider.</param>
        public ZptSharpSelfHoster(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}