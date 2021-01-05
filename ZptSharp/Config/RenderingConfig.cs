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
    /// Represents per-operation configuration information which influences the behaviour of the rendering process.
    /// This class is immutable; use a builder to prepare &amp; build configuration objects in a mutable manner.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Each of the methods of <see cref="IRendersZptFile" /> or <see cref="IRendersZptDocument" /> accepts an
    /// optional rendering configuration parameter, which influences how that rendering operation will behave.
    /// The default for both of these APIs, if configuration is omitted or <see langword="null" />, is to use
    /// a <see cref="Default" /> configuration instance.
    /// </para>
    /// <para>
    /// Because every instance of rendering configuration is immutable, a builder must be used in order to customise
    /// the configuration before it is created.  The primary way to do this is to use the <see cref="CreateBuilder" />
    /// method, then to set up the configuration builder via its settable properties, then finally use
    /// <see cref="RenderingConfig.Builder.GetConfig" /> in order to build and return the rendering configuration.
    /// </para>
    /// <para>
    /// An alternative way to get a customised rendering configuration is to copy the settings/state of an existing
    /// configuration object into a builder.  This is performed via <see cref="CloneToNewBuilder" />, executed from
    /// the configuration object which you wish to copy.  You may then use that builder in the same way as if a new
    /// builder had been created from scratch.
    /// </para>
    /// </remarks>
    /// <example>
    /// <para>
    /// The following logic creates a new rendering configuration which has source annotation enabled.
    /// </para>
    /// <code language="csharp">
    /// var builder = RenderingConfig.CreateBuilder();
    /// builder.IncludeSourceAnnotation = true;
    /// var config = builder.GetConfig();
    /// </code>
    /// <para>
    /// This logic then creates a second rendering configuration based upon the example above
    /// which also uses a default expression type of <c>string</c>.
    /// </para>
    /// <code language="csharp">
    /// var builder2 = config.CloneToNewBuilder();
    /// builder2.DefaultExpressionType = "string";
    /// var config2 = builder2.GetConfig();
    /// </code>
    /// </example>
    public partial class RenderingConfig
    {
        /// <summary>
        /// Gets the encoding which will be used to read &amp; write documents, where the document
        /// provider supports it.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Not all document providers will honour this encoding configuration setting;
        /// it is respected only where the underlying DOM document reader/writer supports it.
        /// Some document providers might only support auto-detection of encoding or might
        /// be hard-coded to always use the same encoding.  Refer to the documentation of
        /// the document provider used - the implementation of <see cref="IReadsAndWritesDocument" /> -
        /// to see if this configuration property is supported.
        /// </para>
        /// </remarks>
        /// <seealso cref="IReadsAndWritesDocument"/>
        /// <value>The document encoding.</value>
        public virtual Encoding DocumentEncoding { get; private set; }

        /// <summary>
        /// Gets the document provider implementation which is to be used for the current rendering task.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When using <see cref="IRendersZptFile"/>, this configuration property is irrelevant.
        /// The file-rendering service will select an appropriate document renderer based upon
        /// the filename &amp; extension of the source file.
        /// </para>
        /// <para>
        /// This configuration property is important when making use of <see cref="IRendersZptDocument"/>,
        /// though.
        /// Because the document-rendering service does not use a file path, there is no filename or
        /// extension to analyse.
        /// When using the document rendering service, this configuration property is used to specify
        /// which document provider implementation should be used to read &amp; write the underlying DOM
        /// document.
        /// </para>
        /// <para>
        /// If the document-rendering service is used and this configuration property is unset then
        /// there is only one other way to avoid the rendering process failing with an exception.
        /// That way is to manually register an implementation of <see cref="IReadsAndWritesDocument"/>
        /// with the <c>IServiceProvider</c> from which the document renderer was resolved.
        /// </para>
        /// </remarks>
        /// <value>The document provider implementation to be used by the document-renderer service.</value>
        public virtual IReadsAndWritesDocument DocumentProvider { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether the XML document declaration should be omitted when
        /// writing XML documents.  Has no effect unless an XML-based document provider is used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This controls XML-style document provider implementations and instructs them whether to write or
        /// whether to omit the <c>&lt;?xml ... ?&gt;</c> declarations on the first line of the file.
        /// </para>
        /// <para>
        /// Obviously, document providers which render HTML and not XML documents will ignore this
        /// configuration setting.
        /// </para>
        /// </remarks>
        /// <value><c>true</c> if the XML document declaration should be omitted; otherwise, <c>false</c>.</value>
        public virtual bool OmitXmlDeclaration { get; private set; }

        /// <summary>
        /// Gets a 'factory delegate' which provides the root contexts for expression resolution.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The root contexts are the variables which are available to all TALES expressions
        /// before any further variables are defined.
        /// They are essentially the pre-defined variables which are already in-scope at the
        /// root of the DOM document.
        /// These variables are available in their own right or also as properties of a special
        /// root contexts object, accessible via the TALES keyword <c>CONTEXTS</c>.
        /// That contexts keyword may not be overridden by any other variable definition, meaning
        /// that the root contexts are always available via that reference.
        /// </para>
        /// <para>
        /// This configuration property allows the use of a customised service which gets those
        /// root contexts.
        /// A developer may write their own implementation of <see cref="IGetsDictionaryOfNamedTalesValues" />
        /// which contains any arbitrary logic to retrieve those variables.
        /// The format of this property is a 'factory delegate' (a function) taking a parameter of
        /// type <see cref="ExpressionContext" /> and returning the implementation of
        /// <see cref="IGetsDictionaryOfNamedTalesValues" />.
        /// </para>
        /// <para>
        /// For the majority of foreseen usages, this configuration property does not need to be used.
        /// If this property is <see langword="null"/> then the rendering process will use a standard
        /// implementation of <see cref="IGetsDictionaryOfNamedTalesValues" /> to retrieve the root
        /// contexts.
        /// In particular, developers do not need to write their own implementation of
        /// <see cref="IGetsDictionaryOfNamedTalesValues" /> in order to simply add extra root
        /// contexts/variables to the rendering process.
        /// In order to add additional variables, developers should use the <see cref="ContextBuilder"/>
        /// configuration property instead.
        /// Passing model data to the rendering process also does not need a customised root contexts
        /// provider.
        /// Model data is passed as a separate parameter to the rendering process and is accessible via
        /// the built-in root context <c>here</c>.
        /// </para>
        /// <para>
        /// Please note that if this property is specified then it is up to the custom implementation of
        /// <see cref="IGetsDictionaryOfNamedTalesValues"/> to include the keyword options in the root
        /// contexts, using the name <c>options</c>.
        /// If the custom implementation does not do this, then the configuration property
        /// <see cref="KeywordOptions"/> will not be honoured.
        /// There are two ways in which a custom implementation may achieve this.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// It may constructor-inject an instance of <see cref="RenderingConfig"/> and include the keyword
        /// options in its own output.
        /// </description></item>
        /// <item><description>
        /// It may derive from the standard implementation of <see cref="IGetsDictionaryOfNamedTalesValues"/>
        /// found in the ZptSharp implementations package and override/supplement its returned results.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <value>The root contexts provider.</value>
        public virtual Func<ExpressionContext, IGetsDictionaryOfNamedTalesValues> RootContextsProvider { get; private set; }

        /// <summary>
        /// Gets a collection of "keyword options" available at the root TALES context named <c>options</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Keyword options are a series of arbitrary name/value pairs.
        /// Their precise semantics is not strictly specified by the ZPT syntax specification.
        /// Typically they could be used to represent command-line arguments in a CLI app.
        /// They should not be used to store model data when the model itself would be more suitable.
        /// </para>
        /// <para>
        /// Note that because <c>options</c> is a root TALES context, if the <see cref="RootContextsProvider"/>
        /// configuration setting is also specified, then that could mean that this setting is not honoured.
        /// It is up to a custom implementation of the root contexts provider to include the keyword options.
        /// </para>
        /// </remarks>
        /// <value>The keyword options collection.</value>
        public virtual IReadOnlyDictionary<string,object> KeywordOptions { get; private set; }

        /// <summary>
        /// Gets a builder action which is used to extend the root contexts, without needing to
        /// replace the <see cref="RootContextsProvider" />.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This configuration setting provides a convenient way to add additional root contexts;
        /// pre-defined variables which are available to the entire rendering process.
        /// Using this setting avoids the need to use a custom <see cref="RootContextsProvider" />
        /// implementation when all that was desired was to add extra variables.
        /// </para>
        /// </remarks>
        /// <value>The context builder action.</value>
        public virtual Action<IConfiguresRootContext, IServiceProvider> ContextBuilder { get; private set; }

        /// <summary>
        /// Gets a value which indicates whether or not source annotation should be written to the rendered document.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Source annotation is a useful ZPT feature used for debugging and understanding the rendering process,
        /// such as when diagnosing a problem.
        /// When source annotation is enabled, each time an element is included from 'elsewhere' (such as via a METAL
        /// macro, or by filling a slot), an HTML/XML comment is added indicating the source information and source
        /// line number from where that element originated.
        /// </para>
        /// </remarks>
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
                RootContextsProvider = RootContextsProvider,
                ContextBuilder = ContextBuilder,
                IncludeSourceAnnotation = IncludeSourceAnnotation,
                KeywordOptions = KeywordOptions.ToDictionary(k => k.Key, v => v.Value),
                OmitXmlDeclaration = OmitXmlDeclaration,
                SourceAnnotationBasePath = SourceAnnotationBasePath,
                DefaultExpressionType = DefaultExpressionType,
            };
        }

        /// <summary>
        /// Creates a new configuration builder object with state equivalent to a default configuration object.
        /// </summary>
        /// <remarks>
        /// <para>
        /// A builder instance created by this method will have the same initial state as <see cref="Default"/>.
        /// See the documentation there for a summary of this default state.
        /// </para>
        /// </remarks>
        /// <returns>A configuration builder with default initial state.</returns>
        public static Builder CreateBuilder() => new Builder();

        /// <summary>
        /// The constructor for <see cref="RenderingConfig"/> is intentionally <see langword="private"/>.
        /// Instances of this class must be created via a <see cref="Builder"/>.
        /// </summary>
        RenderingConfig()
        {
            DocumentEncoding = Encoding.UTF8;
            KeywordOptions = new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
            ContextBuilder = (c, s) => { };
            DefaultExpressionType = WellKnownExpressionPrefix.Path;
        }

        /// <summary>
        /// Gets an instance of <see cref="RenderingConfig"/> with default values.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The property values for a default rendering configuration are as follows.  This applies
        /// to instances created via this static property and also the 'starting' state of a builder
        /// created via <see cref="CreateBuilder"/>.
        /// </para>
        /// <list type="table">
        /// <item>
        /// <term><see cref="DocumentEncoding"/></term>
        /// <description><c>Encoding.UTF8</c></description>
        /// </item>
        /// <item>
        /// <term><see cref="DocumentProvider"/></term>
        /// <description><see langword="null"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="OmitXmlDeclaration"/></term>
        /// <description><see langword="false"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="RootContextsProvider"/></term>
        /// <description><see langword="null"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="KeywordOptions"/></term>
        /// <description>An empty dictionary</description>
        /// </item>
        /// <item>
        /// <term><see cref="ContextBuilder"/></term>
        /// <description>An empty/no-op action</description>
        /// </item>
        /// <item>
        /// <term><see cref="IncludeSourceAnnotation"/></term>
        /// <description><see langword="false"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SourceAnnotationBasePath"/></term>
        /// <description><see langword="null"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="DefaultExpressionType"/></term>
        /// <description>The string <c>path</c></description>
        /// </item>
        /// </list>
        /// </remarks>
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
