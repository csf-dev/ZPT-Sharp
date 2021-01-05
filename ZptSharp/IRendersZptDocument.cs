using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ZptSharp.Config;
using ZptSharp.Rendering;

namespace ZptSharp
{
    /// <summary>
    /// An entry-point object for use by consuming logic.  Renders ZPT documents
    /// from a stream and returns the rendered document as a stream.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Use this interface in your own application if you want to render a ZPT document from
    /// a <c>Stream</c> source but that stream did not originate as a file in a file system.
    /// The method
    /// <see cref="RenderAsync(Stream, object, RenderingConfig, CancellationToken, IDocumentSourceInfo)" />
    /// in this interface will not be able to use a filename/extension to detect an appropriate
    /// document provider.  This means that either you must provide an instance of
    /// <see cref="Dom.IReadsAndWritesDocument" /> via a <see cref="Config.RenderingConfig" />
    /// or you must register an implementation of the <c>IReadsAndWritesDocument</c> service with
    /// dependency injection (an <c>IServiceCollection</c>) yourself.
    /// If you do neither of these then this service will fail with an exception indicating that
    /// it cannot resolve an instance of <c>IReadsAndWritesDocument</c>.
    /// </para>
    /// </remarks>
    public interface IRendersZptDocument
    {
        /// <summary>
        /// Renders a specified ZPT document from a stream using the specified model.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The <paramref name="stream" /> parameter is a <c>Stream</c> containing the content
        /// of the source ZPT document to be rendered.
        /// </para>
        /// <para>
        /// The <paramref name="model" /> parameter is the model which will be rendered-by/bound-to
        /// the document template.  It will be available as the pre-defined TALES variable <c>here</c>
        /// during the rendering process.  This model may be any arbitrary object, as appropriate to
        /// your application/use-case.
        /// </para>
        /// <para>
        /// Passing a <see cref="RenderingConfig" /> in the <paramref name="config" /> parameter is
        /// optional.  If omitted or null then <see cref="RenderingConfig.Default" /> will be used,
        /// which is likely to be suitable for simple usages.  To create a custom rendering
        /// configuration object, either use <see cref="RenderingConfig.CreateBuilder()" />, or
        /// clone an existing config to a builder via <see cref="RenderingConfig.CloneToNewBuilder()" />
        /// and then make amendments before building a new configuration.
        /// </para>
        /// <para>
        /// The cancellation token parameter <paramref name="token" /> may be used during asynchronous
        /// operations in order to abort/cancel the operation before completion.  For more information
        /// about task cancellation see
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource" />.
        /// </para>
        /// <para>
        /// The <paramref name="sourceInfo" /> parameter is optional information about the source of the
        /// <paramref name="stream" /> parameter.  It may be any implementation of <see cref="IDocumentSourceInfo" />,
        /// including your own custom implementation if you wish.  The source info is useful for
        /// providing diagnostic/debugging information about the source of document, particularly if
        /// <see cref="RenderingConfig.IncludeSourceAnnotation" /> is <c>true</c>.
        /// If omitted or null then a default <see cref="UnknownSourceInfo" /> will be used, indicating
        /// that the source of the document is unknown/unspecified.
        /// </para>
        /// <para>
        /// The return of this method is a <c>Task</c> which exposes a <c>Stream</c>.  That stream is
        /// the rendered document.  If you wish to copy this to a <c>TextWriter</c>, for example to save
        /// it to a file, or to write it to another output, then consider using <see cref="IWritesStreamToTextWriter" />.
        /// </para>
        /// </remarks>
        /// <returns>A stream containing the rendered document.</returns>
        /// <param name="stream">The stream containing the document to render.</param>
        /// <param name="model">The model to use for the rendering process.</param>
        /// <param name="config">An optional rendering configuration object.</param>
        /// <param name="token">An object used to cancel the operation if required.</param>
        /// <param name="sourceInfo">The source info for the <paramref name="stream"/>.</param>
        Task<Stream> RenderAsync(Stream stream,
                                 object model,
                                 RenderingConfig config = null,
                                 CancellationToken token = default,
                                 IDocumentSourceInfo sourceInfo = null);
    }
}
