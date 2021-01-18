using System;
using System.Text;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Expressions;
using System.Collections.ObjectModel;

namespace ZptSharp.Config
{
    public partial class RenderingConfig
    {
        /// <summary>
        /// A mutable API with the same properties as <see cref="RenderingConfig"/>.  Allows setting up a desired state
        /// before being used to create an immutable configuration object via <see cref="GetConfig"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Rendering configuration object are immutable and may not be altered once they have been created.
        /// Use a configuration builder to set the properties/configuration settings as desired, before building the
        /// configuration object itself.
        /// </para>
        /// </remarks>
        public class Builder
        {
            readonly RenderingConfig config;
            bool isBuilt;
            IDictionary<string, object> keywordOptions = new Dictionary<string,object>();

            /// <summary>
            /// Gets or sets the encoding which will be used to read &amp; write documents, where the document
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
            public Encoding DocumentEncoding
            {
                get => config.DocumentEncoding;
                set { AssertIsNotBuilt(); config.DocumentEncoding = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// Gets or sets the type of document provider implementation which is to be used for the current rendering task.
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
            public Type DocumentProviderType
            {
                get => config.DocumentProviderType;
                set { AssertIsNotBuilt(); config.DocumentProviderType = value; }
            }

            /// <summary>
            /// Gets or sets a value which indicates whether the XML document declaration should be omitted when
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
            public bool OmitXmlDeclaration
            {
                get => config.OmitXmlDeclaration;
                set { AssertIsNotBuilt(); config.OmitXmlDeclaration = value; }
            }

            /// <summary>
            /// Gets or sets a 'factory delegate' which provides the root contexts for expression resolution.
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
            public Func<ExpressionContext, IGetsDictionaryOfNamedTalesValues> RootContextsProvider
            {
                get => config.RootContextsProvider;
                set { AssertIsNotBuilt(); config.RootContextsProvider = value; }
            }

            /// <summary>
            /// Gets or sets a collection of name/value pairs available at the root TALES context (variable) named <c>options</c>.
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
            public IDictionary<string, object> KeywordOptions
            {
                get => keywordOptions;
                set { AssertIsNotBuilt(); keywordOptions = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// Gets or sets a value which indicates whether or not source annotation should be written to the rendered document.
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
            public bool IncludeSourceAnnotation
            {
                get => config.IncludeSourceAnnotation;
                set { AssertIsNotBuilt(); config.IncludeSourceAnnotation = value; }
            }

            /// <summary>
            /// Gets or sets a callback which is used to extend the root contexts, without needing to
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
            public Action<IConfiguresRootContext, IServiceProvider> ContextBuilder
            {
                get => config.ContextBuilder;
                set { AssertIsNotBuilt(); config.ContextBuilder = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// Gets or sets a file system path which is used as the base path to shorten source annotation filenames,
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
            public string SourceAnnotationBasePath
            {
                get => config.SourceAnnotationBasePath;
                set { AssertIsNotBuilt(); config.SourceAnnotationBasePath = value; }
            }

            /// <summary>
            /// Gets or sets the default expression type name/prefix, used for TALES expressions which do not have a prefix.
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
            public string DefaultExpressionType
            {
                get => config.DefaultExpressionType;
                set { AssertIsNotBuilt(); config.DefaultExpressionType= value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// Gets or sets a custom XML URL resolver which should be used to resolve XML namespaces
            /// for XML-based document providers.
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
            public System.Xml.XmlUrlResolver XmlUrlResolver
            {
                get => config.XmlUrlResolver;
                set { AssertIsNotBuilt(); config.XmlUrlResolver = value; }
            }

            /// <summary>
            /// Gets or sets a value which indicates whether or not simple value substitutions using <c>${expression}</c>
            /// are activated.
            /// </summary>
            /// <remarks>
            /// <para>
            /// When this configuration option is <c>true</c> then in markup text and also attribute values, substitutions
            /// will be processed using placeholders in the format <c>${expression}</c>.  Where this pattern is detected in
            /// text content, the whole construct will be replaced with the result of the expression, similarly to if it were
            /// an element using a <c>tal:replace</c> attribute. This has the exception that the <c>structure</c> &amp; <c>text</c>
            /// prefixes are not permitted.  You may use <c>structure:</c> expressions to achieve the same effect, if you wish.
            /// </para>
            /// <para>
            /// Also, then this option is <c>true</c> then the same pattern will be detected and replaced in the values of
            /// attributes.
            /// </para>
            /// <para>
            /// <c>${expression}</c> replacements WILL NOT be processed in text content for the immediate text-node children of
            /// an element which has either a <c>tal:content</c> or <c>tal:replace</c> attribute.  Additionally, replacements
            /// will not be made in attribute values if there is a <c>tal:attributes</c> attribute present upon the same element.
            /// This does not matter which attributes are listed/mentioned in the <c>tal:attributes</c> attribute.  The presence of
            /// the TAL attribute prevents all simple replacements for all other attributes upon the same element.
            /// </para>
            /// <para>
            /// Enabling this option may cause a performance degradation, particularly for applications which include large/long
            /// text nodes, for example paragraphs/articles of text.  This is because these text nodes must additionally be scanned
            /// for the replacement pattern and processed.  The same functionality may be achieved with improve performance using
            /// <c>tal:replace</c> attributes, without activating this option.
            /// </para>
            /// <para>
            /// When using simple replacements in both text and/or attribute values, the replacement sequence <c>${...}</c> may
            /// be escaped by preceding it with a single backslash: <c>\${...}</c>.  In this case, replacements will not be made,
            /// the backslash will be removed and that is all.
            /// </para>
            /// </remarks>
            public bool UseSimpleValueSubstitutions
            {
                get => config.UseSimpleValueSubstitutions;
                set { AssertIsNotBuilt(); config.UseSimpleValueSubstitutions = value; }
            }

            /// <summary>
            /// Builds and returns a configuration object based on the state of the current builder instance.
            /// This method may be used only once per builder instance.
            /// </summary>
            /// <remarks>
            /// <para>
            /// Rendering configuration objects (as returned by this method) are immutable and may not be modified
            /// in any way after they are created.
            /// </para>
            /// <para>
            /// In addition, this method may be used only once per builder instance.  After a configuration object has
            /// been created, the builder which created it is no longer usable.
            /// It is possible to build another configuration object based upon an existing one, though, by use of
            /// <see cref="RenderingConfig.CloneToNewBuilder"/>.
            /// </para>
            /// </remarks>
            /// <returns>The immutable rendering configuration.</returns>
            public RenderingConfig GetConfig()
            {
                AssertIsNotBuilt();
                isBuilt = true;

                config.KeywordOptions = new ReadOnlyDictionary<string, object>(KeywordOptions);
                return config;
            }

            void AssertIsNotBuilt()
            {
                if (isBuilt)
                    throw new InvalidOperationException($"An instance of {nameof(RenderingConfig)}.{nameof(Builder)} may be used to build only " +
                                                        "one configuration object, and that object is immutable once it has been built.  To create a new configuration, " +
                                                        "create a new builder.");
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder"/> class.
            /// </summary>
            internal Builder()
            {
                config = new RenderingConfig();
            }
        }
    }
}
