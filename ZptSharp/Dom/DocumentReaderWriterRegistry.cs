using System;
using System.Collections.Generic;
using System.Linq;
using ZptSharp.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An implementation of <see cref="IGetsDocumentReaderWriterForFile"/> which makes use
    /// of <see cref="Hosting.EnvironmentRegistry"/> and a <see cref="IServiceProvider"/>
    /// to get instances of <see cref="IReadsAndWritesDocument"/> for a given filename or path.
    /// </summary>
    public class DocumentReaderWriterRegistry : IGetsDocumentReaderWriterForFile
    {
        readonly Hosting.EnvironmentRegistry typeRegistry;
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Gets the document reader/writer for the specified filename.
        /// </summary>
        /// <returns>The document reader/writer.</returns>
        /// <param name="filenameOrPath">A document filename (optionally with its full path).</param>
        public IReadsAndWritesDocument GetDocumentProvider(string filenameOrPath)
            => GetAllProviders().FirstOrDefault(x => x.CanReadWriteForFilename(filenameOrPath));

        IEnumerable<IReadsAndWritesDocument> GetAllProviders()
            => typeRegistry.DocumentProviderTypes.Select(GetProvider).ToList();

        IReadsAndWritesDocument GetProvider(Type x)
            => (IReadsAndWritesDocument) serviceProvider.GetRequiredService(x);

        /// <summary>
        /// Initializes a new instance of <see cref="DocumentReaderWriterRegistry"/>.
        /// </summary>
        /// <param name="typeRegistry">The document provider types registry.</param>
        /// <param name="serviceProvider">A service provider.</param>
        public DocumentReaderWriterRegistry(EnvironmentRegistry typeRegistry, IServiceProvider serviceProvider)
        {
            this.typeRegistry = typeRegistry ?? throw new ArgumentNullException(nameof(typeRegistry));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }
    }
}
