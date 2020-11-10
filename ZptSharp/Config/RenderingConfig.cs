using System;
using System.Text;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Expressions;

namespace ZptSharp.Config
{
    /// <summary>
    /// Configuration for a single instance of <see cref="IRendersZptDocument"/>.  One configuration
    /// object is used for all operations of a single renderer object.
    /// </summary>
    public partial class RenderingConfig
    {
        /// <summary>
        /// Gets an object which provides service-resolution/dependency injection for ZPT Sharp types.
        /// If this is unset or <see langword="null"/> then a default resolution service will be used.
        /// </summary>
        /// <value>The service provider.</value>
        public virtual IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Gets the encoding which will be used to read &amp; write documents, where the document
        /// provider supports it (not all do).
        /// If this is unset then documents will be read &amp; written as UTF-8.
        /// </summary>
        /// <value>The document encoding.</value>
        public virtual Encoding DocumentEncoding { get; private set; }

        /// <summary>
        /// <para>
        /// Gets the document provider to be used for reading/writing documents.
        /// </para>
        /// <para>
        /// This property is not used if the <see cref="ServiceProvider"/> has been set to anything
        /// other than the default (<see langword="null"/>) value.  If a custom service provider is
        /// used then the document provider must be resolvable from that service provider.
        /// </para>
        /// </summary>
        /// <value>The document provider.</value>
        public virtual IReadsAndWritesDocument DocumentProvider { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the XML document declaration should be omitted when
        /// writing XML documents.  Has no effect unless an XML-based document provider is used.
        /// </summary>
        /// <value><c>true</c> if the XML document declaration should be omitted; otherwise, <c>false</c>.</value>
        public virtual bool OmitXmlDeclaration { get; private set; }

        /// <summary>
        /// Gets a service which provides the default/built-in root contexts for expression resolution.
        /// </summary>
        /// <value>The built-in contexts provider.</value>
        public virtual Func<ExpressionContext,IGetsNamedTalesValue> BuiltinContextsProvider { get; private set; }

        /// <summary>
        /// Gets a collection of "keyword options" which have been provided to the rendering process externally.
        /// </summary>
        /// <value>The keyword options collection.</value>
        public virtual IDictionary<string,object> KeywordOptions { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether or not source annotation should be written to the rendered document.
        /// Source annotation adds comments to the output indicating the source file information.
        /// </summary>
        /// <value><c>true</c> if source annotation should be included in the output; otherwise, <c>false</c>.</value>
        public virtual bool IncludeSourceAnnotation { get; private set; }

        /// <summary>
        /// <para>
        /// The constructor for <see cref="RenderingConfig"/> is intentionally <see langword="protected"/>.
        /// Instances of this class must be created via an instance of <see cref="Builder"/>.
        /// </para>
        /// <example>
        /// <para>
        /// Here is an example showing how to create a new config object.
        /// </para>
        /// <code>
        /// var builder = new RenderingConfig.Builder();
        /// /* Use the builder to set config values */
        /// var config = builder.GetConfig();
        /// </code>
        /// </example>
        /// </summary>
        protected RenderingConfig() {}

        /// <summary>
        /// Gets an instance of <see cref="RenderingConfig"/> with default values.
        /// </summary>
        /// <value>The default rendering config.</value>
        public static RenderingConfig Default
        {
            get
            {
                var builder = new Builder();
                return builder.GetConfig();
            }
        }
    }
}
