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
    /// document provider.
    /// </para>
    /// <para>
    /// This means that you must explicitly select a document provider: an implementation of
    /// <see cref="Dom.IReadsAndWritesDocument" /> and pass it to this document renderer service.
    /// There are two ways to achieve that; the first way is to pass a <see cref="Config.RenderingConfig" />
    /// which has the selected document provider implementation in its <see cref="Config.RenderingConfig.DocumentProvider" />
    /// property.
    /// The other way is to have manually-registered the document provider with the <c>IServiceProvider</c>
    /// from which this document-renderer instance was resolved.
    /// </para>
    /// <para>
    /// If you do neither of the above then <see cref="RenderAsync(Stream, object, RenderingConfig, CancellationToken, IDocumentSourceInfo)"/>
    /// will fail with an exception, stating that it cannot resolve an instance of <c>IReadsAndWritesDocument</c>.
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
        /// optional but recommended.
        /// Because this service cannot rely upon a filename/extension to automatically select an
        /// appropriate document provider, using a configuration object with the <see cref="Config.RenderingConfig.DocumentProvider"/>
        /// property set is the easiest way to explicitly select the provider.
        /// The alternative is to have registered an implementation of <see cref="Dom.IReadsAndWritesDocument" />
        /// with the same <c>IServiceProvider</c> as was used to resolve this document renderer instance.
        /// </para>
        /// <para>
        /// If the configuration is omitted or null then <see cref="RenderingConfig.Default" /> will be
        /// used, which has mostly sane defaults, except does not specify a document provider (see above).
        /// </para>
        /// <para>
        /// The cancellation token parameter <paramref name="token" /> may be used during asynchronous
        /// operations in order to abort/cancel the operation before completion.  For more information
        /// about task cancellation see
        /// <see href="https://docs.microsoft.com/en-us/dotnet/api/system.threading.cancellationtokensource" />.
        /// </para>
        /// <para>
        /// The <paramref name="sourceInfo" /> parameter is optional information about the source of the
        /// <paramref name="stream" /> parameter.  It may be any implementation of <see cref="IDocumentSourceInfo" />.
        /// The source info is useful for providing diagnostic/debugging information, particularly if
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
        /// <param name="stream">A stream containing the document to render.</param>
        /// <param name="model">The model/data to be rendered by the document.</param>
        /// <param name="config">An optional rendering configuration.</param>
        /// <param name="token">An optional token used to cancel/abort the operation whilst it is in-progress.</param>
        /// <param name="sourceInfo">An optional source information object describing the source of the <paramref name="stream"/>.</param>
        Task<Stream> RenderAsync(Stream stream,
                                 object model,
                                 RenderingConfig config = null,
                                 CancellationToken token = default,
                                 IDocumentSourceInfo sourceInfo = null);
    }
}
