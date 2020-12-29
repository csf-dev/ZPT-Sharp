using System;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// An implementation of <see cref="IHostsZptSharp" /> which wraps an
    /// <see cref="IServiceProvider" /> and uses that to provide the entry-points.
    /// </summary>
    public class ZptSharpSelfHoster : IHostsZptSharp
    {
        readonly IServiceProvider serviceProvider;
        bool disposedValue;

        /// <summary>
        /// Gets a service which renders ZPT template files from disk paths.
        /// </summary>
        /// <value>The file renderer.</value>
        public IRendersZptFile FileRenderer => serviceProvider.GetRequiredService<IRendersZptFile>();

        /// <summary>
        /// Gets a service which renders ZPT templates from streams (whether they came from a file or not).
        /// </summary>
        /// <value>The document renderer.</value>
        public IRendersZptDocument DocumentRenderer => serviceProvider.GetRequiredService<IRendersZptDocument>();

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
                if (disposing && serviceProvider is IDisposable disposableProvider)
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
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}