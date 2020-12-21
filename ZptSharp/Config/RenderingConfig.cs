using System;
using System.Text;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using System.Linq;
using System.Collections.ObjectModel;

namespace ZptSharp.Config
{
    /// <summary>
    /// Configuration for a single instance of <see cref="IRendersZptDocument"/>.  One configuration
    /// object is used for all operations of a single renderer object.
    /// </summary>
    public partial class RenderingConfig
    {
        /// <summary>
        /// Gets the encoding which will be used to read &amp; write documents, where the document
        /// provider supports it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Not all document providers (see also: <seealso cref="IReadsAndWritesDocument"/>) will
        /// honour this document encoding configuration setting, it is up to the individual provider used
        /// as to how this setting is treated.
        /// </para>
        /// <para>
        /// The default value for this configuration setting is <see cref="Encoding.UTF8"/>.
        /// </para>
        /// </remarks>
        /// <value>The document encoding.</value>
        public virtual Encoding DocumentEncoding { get; private set; }

        /// <summary>
        /// Gets the document provider; this is the implementation used for reading/writing documents.
        /// </summary>
        /// <value>The document provider.</value>
        public virtual IReadsAndWritesDocument DocumentProvider { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the XML document declaration should be omitted when
        /// writing XML documents.  Has no effect unless an XML-based document provider is used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default for this configuration option is <see langword="false"/>.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> if the XML document declaration should be omitted; otherwise, <c>false</c>.</value>
        public virtual bool OmitXmlDeclaration { get; private set; }

        /// <summary>
        /// Gets a service which provides the default/built-in root contexts for expression resolution.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The default contexts provider is the object which is used to provide the 'root'
        /// variables which are available to TALES path expressions.  These built-in contexts
        /// are also accessible via the keyword <c>CONTEXTS</c> in a path expression.
        /// </para>
        /// <para>
        /// For the vast majority of usages, the standard/default built-in contexts provider will be suitable.
        /// That standard/default implementation will be used if this configuration option is either not
        /// specified or is set to <see langword="null"/>.
        /// </para>
        /// <para>
        /// Specify a built-in contexts provider when you wish to use a non-standard mechanism of
        /// getting the 'root' variables, accessible to document templates.  Please note, though,
        /// that you don't need to do this to pass model data or to simply add extra root variables.
        /// Model data is easily provided by passing a model value into the rendering process.  The
        /// passed value is accessible via the built-in context keyword <c>here</c>.
        /// Adding additional root variables is achieved by the use of the <see cref="ContextBuilder"/>
        /// configuration option.
        /// </para>
        /// <para>
        /// Please note that if this configuration option is specified and is not null then
        /// the <see cref="KeywordOptions"/> configuration option will be ignored
        /// and will have no effect.  This is because those keyword options are made accessible
        /// to templates via the built-in contexts provider.
        /// </para>
        /// </remarks>
        /// <value>The built-in contexts provider.</value>
        public virtual Func<ExpressionContext, IGetsDictionaryOfNamedTalesValues> BuiltinContextsProvider { get; private set; }

        /// <summary>
        /// <para>
        /// Gets a collection of "keyword options".  A typical usage of keyword options is to
        /// represent key/value pairs passed as arguments to a command-line application.
        /// </para>
        /// <para>
        /// Beware that this configuration option is ignored if <see cref="BuiltinContextsProvider"/> is
        /// also specified and is not null.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// 
        /// </para>
        /// </remarks>
        /// <value>The keyword options collection.</value>
        public virtual IReadOnlyDictionary<string,object> KeywordOptions { get; private set; }

        /// <summary>
        /// Gets an action which is used to build &amp; add values to the root ZPT context.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If unset or null, then no additional variables will be added to the root context.
        /// If this is provided then the <see cref="IConfiguresRootContext"/> may be used
        /// to add additional data, accessible by templates as variables, to the rendering
        /// process.
        /// </para>
        /// </remarks>
        /// <value>The context builder.</value>
        public virtual Action<IConfiguresRootContext, IServiceProvider> ContextBuilder { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether or not source annotation should be written to the rendered document.
        /// Source annotation adds comments to the output indicating the source file information.
        /// </summary>
        /// <value><c>true</c> if source annotation should be included in the output; otherwise, <c>false</c>.</value>
        public virtual bool IncludeSourceAnnotation { get; private set; }

        /// <summary>
        /// <para>
        /// Gets a file system path which is used as the 'base' or root path for template files.
        /// When <see cref="IncludeSourceAnnotation"/> is <see langword="true"/>, if this property is
        /// non-null, file paths to template files are recorded as relative paths based upon this path.
        /// </para>
        /// <para>
        /// If this property is <see langword="null"/> or where a template file is not a descendent of this
        /// path, then full absolute file paths will be recorded in source annotation.
        /// </para>
        /// <para>
        /// This property has no effect if <see cref="IncludeSourceAnnotation"/> is <see langword="false"/>.
        /// </para>
        /// </summary>
        /// <value>The base path used for shorting the paths of source files in logs and source annotation.</value>
        public virtual string SourceAnnotationBasePath { get; private set; }

        /// <summary>
        /// Gets the default expression type name/prefix, used for TALES expressions which do not have a prefix.
        /// If left unset, this defaults to <see cref="WellKnownExpressionPrefix.Path"/>.
        /// </summary>
        /// <value>The default expression-type prefix.</value>
        public virtual string DefaultExpressionType { get; private set; }

        /// <summary>
        /// <para>
        /// Gets a copy of the current configuration instance, returned as a
        /// <see cref="Builder"/> object, allowing further amendments.
        /// </para>
        /// <para>
        /// This does not allow alterations to the current configuration
        /// instance; configurations are immutable once built.  Rather it creates
        /// and returns a builder pre-populated with the same settings as the
        /// current configuration instance.
        /// </para>
        /// </summary>
        /// <returns>A configuration builder.</returns>
        public Builder CloneToNewBuilder()
        {
            return new Builder
            {
                DocumentEncoding = DocumentEncoding,
                DocumentProvider = DocumentProvider,
                BuiltinContextsProvider = BuiltinContextsProvider,
                ContextBuilder = ContextBuilder,
                IncludeSourceAnnotation = IncludeSourceAnnotation,
                KeywordOptions = KeywordOptions.ToDictionary(k => k.Key, v => v.Value),
                OmitXmlDeclaration = OmitXmlDeclaration,
                SourceAnnotationBasePath = SourceAnnotationBasePath,
                DefaultExpressionType = DefaultExpressionType,
            };
        }

        /// <summary>
        /// Creates a new, empty, configuration builder object.
        /// </summary>
        /// <returns>A configuration builder.</returns>
        public static Builder CreateBuilder() => new Builder();

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
        protected RenderingConfig()
        {
            DocumentEncoding = Encoding.UTF8;
            KeywordOptions = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
            ContextBuilder = (c, s) => { };
            DefaultExpressionType = WellKnownExpressionPrefix.Path;
        }

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
