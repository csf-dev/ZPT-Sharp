using System;
using ZptSharp.Rendering;

namespace ZptSharp.Hosting
{
    /// <summary>
    /// An object which is capable of hosting a ZptSharp environment, using its own
    /// dependency injection, separate from those of the containing application.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this object when you wish to make use of the the entry-points to ZptSharp,
    /// without depending upon ZptSharp being wired up into your applications's dependency
    /// injection.
    /// </para>
    /// </remarks>
    public interface IHostsZptSharp : IDisposable
    {
        /// <summary>
        /// Gets a service which renders ZPT template files from disk paths.
        /// </summary>
        /// <value>The file renderer.</value>
        IRendersZptFile FileRenderer { get; }

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
        IGetsZptDocumentRendererForFilePath DocumentRendererForPathFactory { get; }

        /// <summary>
        /// Gets a service which copies a <see cref="System.IO.Stream" /> to a
        /// <see cref="System.IO.TextWriter" />.
        /// </summary>
        /// <value>The stream-copying service.</value>
        IWritesStreamToTextWriter StreamCopier { get; }
    }
}