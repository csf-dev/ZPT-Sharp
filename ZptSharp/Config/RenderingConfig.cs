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
    /// Because every instance of rendering configuration is immutable, a builder object must be used to set-up
    /// the configuration object before it is used.
    /// The primary way to do this is:
    /// </para>
    /// <list type="number">
    /// <item><description>Use the <see cref="CreateBuilder" /> method to get a builder</description></item>
    /// <item><description>Alter the settings as desired upon the builder</description></item>
    /// <item><description>Use <see cref="RenderingConfig.Builder.GetConfig" /> to build the configuration object</description></item>
    /// </list>
    /// <para>
    /// An alternative way to get a customised rendering configuration is to copy the settings/state of an existing
    /// configuration object into a builder.  This is performed via <see cref="CloneToNewBuilder" />.
    /// You may then use that builder from step 2 onward in the process described above, except that it will be
    /// pre-filled with the same state as in the cloned configuration.
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
        /// Refer to the documentation of the document provider used - the implementation of
        /// <see cref="IReadsAndWritesDocument" /> - to see if this configuration property is supported.
        /// </para>
        /// <para>
        /// Where supported, this configuration setting allows the reading &amp; writing of
        /// ZPT documents which use different character encodings.
        /// </para>
        /// <para>
        /// In all modern cases, of course, UTF8 is the recommended encoding.
        /// </para>
        /// </remarks>
        /// <seealso cref="IReadsAndWritesDocument"/>
        /// <value>The document encoding.</value>
        public virtual Encoding DocumentEncoding { get; private set; }

        /// <summary>
        /// Gets the type of document provider implementation which is to be used for the current rendering task.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When using <see cref="IRendersZptFile"/>, this configuration property is irrelevant and ignored.
        /// The file-rendering service will select an appropriate document renderer type based upon
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
        /// </remarks>
        /// <seealso cref="IRendersZptDocument"/>
        /// <value>The document provider implementation type to be used by the document-renderer service.</value>
        public virtual Type DocumentProviderType { get; private set; }

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
        /// Gets a collection of name/value pairs available at the root TALES context (variable) named <c>options</c>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Keyword options are a series of arbitrary name/value pairs.
        /// Their precise semantics are loosely-defined by the ZPT syntax specification and the functionality
        /// is very rarely-used in real-life implementations, nor is it recommended to begin using them if not
        /// absolutely neccesary.
        /// Keyword options could, for example, be used to contain arbitrary name/value arguments passed to a
        /// command-line app.
        /// In all foreseeable use-cases though, the model is a far better way to make data available
        /// to document templates.
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
        /// Gets a callback which is used to extend the root contexts, without needing to
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
        /// When source annotation is enabled, each time there is an insertion of markup from a different source, an
        /// HTML/XML comment is added indicating that source and source line number.
        /// The insertion of markup most commonly refers to the usage of METAL macros and the filling of slots.
        /// It helps developers confidently understand "where did this output come from"?
        /// </para>
        /// <para>
        /// The "source" for any document is simplest when <see cref="IRendersZptFile"/> is used, since it
        /// is quite simply the path to the file, relative to <see cref="SourceAnnotationBasePath"/> where applicable.
        /// When <see cref="IRendersZptDocument"/> is used instead then a source info object would be passed (via
        /// optional parameter) to
        /// <see cref="IRendersZptDocument.RenderAsync(System.IO.Stream, object, RenderingConfig, System.Threading.CancellationToken, Rendering.IDocumentSourceInfo)"/>.
        /// A custom implementation of <see cref="Rendering.IDocumentSourceInfo"/> could represent anything,
        /// such as a database key, API URI or whatever application-specific information is applicable.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// Here is a sample of what source annotation could look like the following in the rendered output.
        /// It appears as an HTML or XML comment, designated by a block of <c>=</c> symbols.
        /// It then shows the string representation of the source information and the line number at which
        /// the included content originated.
        /// </para>
        /// <code language="html">
        /// &lt;span&gt;This span element was originally defined in "MyMacro.pt", line 4&lt;/span&gt;&lt;!--
        /// ==============================================================================
        /// MySourceFiles\MyMacro.pt (line 4)
        /// ==============================================================================
        /// --&gt;
        ///     This is further content in the rendered output document.
        /// </code>
        /// </example>
        /// <seealso cref="Rendering.IDocumentSourceInfo"/>
        /// <seealso cref="SourceAnnotationBasePath"/>
        /// <value><c>true</c> if source annotation should be included in the output; otherwise, <c>false</c>.</value>
        public virtual bool IncludeSourceAnnotation { get; private set; }

        /// <summary>
        /// Gets a file system path which is used as the base path to shorten source annotation filenames,
        /// when <see cref="IncludeSourceAnnotation"/> is <see langword="true"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This configuration setting is only relevant when <see cref="IncludeSourceAnnotation"/> is <see langword="true"/>
        /// and also when documents are rendered from file system files, such as when <see cref="IRendersZptFile"/>
        /// is being used.
        /// In any other scenario this configuration setting will not be used and will have no effect.
        /// </para>
        /// <para>
        /// When source annotation comments are added to the rendered output, if the source of a document is
        /// a filesystem file, the comment will include the path to that file.
        /// If this configuration setting is not specified or is <see langword="null"/> then the full,
        /// absolute file path will be recorded in the comment.
        /// If this configuration setting is specified, and the source file (receiving the source annotation
        /// comment) is a descendent of this base path, then only the relative portion of the file path will
        /// be recorded in the source annotation comment.
        /// </para>
        /// <para>
        /// If the document file receiving the source annotation comment is not a descendent of this base path,
        /// then the full absolute path will still be used.
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// These examples show a few combinations of what would be written in the source annotation comments for
        /// various scenarios.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If <see cref="IncludeSourceAnnotation"/> is <see langword="false"/> then no source annotation would be
        /// written at all, and this setting (and all other scenarios listed here) would be meaningless.
        /// </description></item>
        /// <item><description>
        /// If the source of the documents involved is not a file from a file system: <see cref="Rendering.FileSourceInfo"/>
        /// then this configuration setting is meaningless.  The source information written to the comments would
        /// always come directly from the implementation of <see cref="Rendering.IDocumentSourceInfo"/>.
        /// </description></item>
        /// <item><description>
        /// If the source of the document were <c>C:\MyDirectory\MyFile.html</c> and this configuration setting is
        /// <see langword="null"/> then source annotation comments would refer to that document using the path
        /// <c>C:\MyDirectory\MyFile.html</c>.
        /// </description></item>
        /// <item><description>
        /// If the source of the document were <c>C:\MyDirectory\MyFile.html</c> and this configuration setting is
        /// <c>C:\MyDirectory</c> then source annotation comments would refer to that document using the path
        /// <c>MyFile.html</c>.
        /// </description></item>
        /// <item><description>
        /// If the source of the document were <c>C:\MyDirectory\MySubDir\MyFile.html</c> and this configuration setting is
        /// <c>C:\MyDirectory</c> then source annotation comments would refer to that document using the path
        /// <c>MySubDir\MyFile.html</c>.
        /// </description></item>
        /// <item><description>
        /// If the source of the document were <c>C:\OtherDirectory\MyFile.html</c> and this configuration setting is
        /// <c>C:\MyDirectory</c> then source annotation comments would refer to that document using the path
        /// <c>C:\OtherDirectory\MyFile.html</c>.
        /// The absolute path would be used because the source file is outside the base path.
        /// </description></item>
        /// </list>
        /// </example>
        /// <seealso cref="Rendering.IDocumentSourceInfo"/>
        /// <seealso cref="Rendering.FileSourceInfo"/>
        /// <seealso cref="IRendersZptFile"/>
        /// <seealso cref="IncludeSourceAnnotation"/>
        /// <value>The base path used for shorting the paths of source files in logs and source annotation.</value>
        public virtual string SourceAnnotationBasePath { get; private set; }

        /// <summary>
        /// Gets the default expression type name/prefix, used for TALES expressions which do not have a prefix.
        /// </summary>
        /// <remarks>
        /// <para>
        /// In TALES expressions used by the ZPT rendering process, prefixing the expression with an expression-type
        /// is optional.  The rendering process always has a configured default expression type.  Unprefixed expressions
        /// are assumed to be of that default type.
        /// </para>
        /// <para>
        /// This configuration setting permits the changing of that default expression type.
        /// </para>
        /// </remarks>
        /// <value>The default expression-type prefix.</value>
        public virtual string DefaultExpressionType { get; private set; }

        /// <summary>
        /// Gets a custom XML URL resolver which should be used to resolve XML namespaces for XML-based document providers.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When using an XML-based document provider (and only when using an XML-based document provider),
        /// in order to fully validate these documents (and provide appropriate entity support), supporting
        /// assets are required.  These can include DTDs, modules and the like.
        /// When making use of an XML document which conforms to a DTD, it is usually desirable (for both performance
        /// and security purposes) to use a custom XML URL resolver.  This allows techniques such as caching,
        /// security-enforcement and perhaps even the local serving of those assets without making any
        /// HTTP(s) requests at all.
        /// </para>
        /// <para>
        /// When set, this configuration setting specifies the custom XML URL resolver which should be used by
        /// XML-based document providers.  It has no effect at all upon HTML-based document providers.
        /// </para>
        /// <para>
        /// Please note that the official ZptSharp XML document provider includes a URL provider which serves
        /// XHTML assets from embedded resources, bypassing all HTTP requests.  This built-in URL provider will
        /// be used if this configuration setting is <see langword="null"/>.
        /// </para>
        /// </remarks>
        public virtual System.Xml.XmlUrlResolver XmlUrlResolver { get; private set; }

        /// <summary>
        /// Creates and returns a new <see cref="RenderingConfig.Builder"/> instance which has its initial
        /// state/settings copied from the current configuration instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Use this method to create a new configuration builder which is based upon the current configuration,
        /// but may then be modified before it is used to create a new configuration.
        /// Rendering configuration objects are immutable and cannot be altered after they have been built/created.
        /// This method is a convenience to allow the next-best-thing: creating a new builder, cloned from an
        /// existing configuration.
        /// </para>
        /// <para>
        /// This method does not allow editing of the configuration from which the builder was cloned.
        /// Changes made to the returned builder object will not affect the original configuration object.
        /// </para>
        /// </remarks>
        /// <returns>A configuration builder.</returns>
        public Builder CloneToNewBuilder()
        {
            return new Builder
            {
                DocumentEncoding = DocumentEncoding,
                DocumentProviderType = DocumentProviderType,
                RootContextsProvider = RootContextsProvider,
                ContextBuilder = ContextBuilder,
                IncludeSourceAnnotation = IncludeSourceAnnotation,
                KeywordOptions = KeywordOptions.ToDictionary(k => k.Key, v => v.Value),
                OmitXmlDeclaration = OmitXmlDeclaration,
                SourceAnnotationBasePath = SourceAnnotationBasePath,
                DefaultExpressionType = DefaultExpressionType,
                XmlUrlResolver = XmlUrlResolver,
            };
        }

        /// <summary>
        /// Creates a new configuration builder object with a default set of state.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The initial state of the builder object returned by this method will be the same as the state
        /// which would be created by <see cref="Default"/>.  Refer to the documentation of the default
        /// property to see what that state would be.
        /// </para>
        /// <para>
        /// The builder may be used to set-up the intended state of a configuration object.
        /// Use <see cref="Builder.GetConfig"/> to build and return a configuration object from that builder,
        /// once the desired settings have been made.
        /// </para>
        /// </remarks>
        /// <seealso cref="Default"/>
        /// <returns>A configuration builder with default initial state.</returns>
        public static Builder CreateBuilder() => new Builder();

        /// <summary>
        /// The constructor for <see cref="RenderingConfig"/> is intentionally <see langword="private"/>.
        /// Instances of this class must be created via a <see cref="Builder"/>.
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
        /// <term><see cref="DocumentProviderType"/></term>
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
        /// <item>
        /// <term><see cref="XmlUrlResolver"/></term>
        /// <description><see langword="null"/></description>
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
