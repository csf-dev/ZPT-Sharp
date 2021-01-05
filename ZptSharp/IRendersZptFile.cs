using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;

namespace ZptSharp
{
    /// <summary>
    /// An entry-point object for use by consuming logic.  Renders ZPT documents
    /// from a filesystem file and returns the rendered document as a stream.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this interface in your own application if you want to render ZPT documents from
    /// source files which can be described by file paths.  The
    /// <see cref="RenderAsync(string, object, RenderingConfig, CancellationToken)" /> method
    /// will use the filename/extension to detect which document provider is appropriate for
    /// the file and automatically select from those which have been registered.
    /// </para>
    /// </remarks>
    public interface IRendersZptFile
    {
        /// <summary>
        /// Renders a specified ZPT document file using the specified model.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <paramref name="filePath" /> parameter should be the disk or network share path
        /// to the source document file which is to be rendered.  This may be a an absolute
        /// path or it may be relative to the current working directory.
        /// </para>
        /// <para>
        /// The <paramref name="model" /> parameter is the model which will be rendered-by/bound-to
        /// the document template.  It will be available as the pre-defined TALES variable <c>here</c>
        /// during the rendering process.
        /// </para>
        /// <para>
        /// Passing a <see cref="RenderingConfig" /> in the <paramref name="config" /> parameter is
        /// optional.  If omitted or null then this will use a default rendering configuration which
        /// is suitable for most usages.  To create a rendering configuration object, either use
        /// <see cref="RenderingConfig.CreateBuilder()" />, or clone an existing config to a builder
        /// via <see cref="RenderingConfig.CloneToNewBuilder()" />.
        /// </para>
        /// <para>
        /// The cancellation token parameter <paramref name="token" /> may be used during asynchronous
        /// operations in order to abort/cancel the operation before completion.  For more information
        /// about task cancellation see https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource
        /// </para>
        /// <para>
        /// The return of this method is a <c>Task</c> which exposes a <c>Stream</c>.  That stream is
        /// the rendered document.  If you wish to copy this to a <c>TextWriter</c>, for example to save
        /// it to a file, or to write it to another output, then consider using <see cref="IWritesStreamToTextWriter" />.
        /// </para>
        /// </remarks>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="filePath">The path of the document file to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="config">An optional rendering configuration object.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        Task<Stream> RenderAsync(string filePath,
                                 object model,
                                 RenderingConfig config = null,
                                 CancellationToken token = default);
    }
}
