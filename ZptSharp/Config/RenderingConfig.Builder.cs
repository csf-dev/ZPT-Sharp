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
        /// Allows creation of an instance of <see cref="RenderingConfig"/> and provides an API by which
        /// its properties may be set.  Once the state has been populated as desired, use <see cref="GetConfig()"/>
        /// to get the actual configuration instance.
        /// </summary>
        public class Builder
        {
            readonly RenderingConfig config;
            bool isBuilt;
            IDictionary<string, object> keywordOptions = new Dictionary<string,object>();

            /// <summary>
            /// Gets or sets the encoding which will be used to read &amp; write documents, where the document
            /// provider supports it (not all do).
            /// If this is unset then documents will be read &amp; written as UTF-8.
            /// </summary>
            /// <value>The document encoding.</value>
            public Encoding DocumentEncoding
            {
                get => config.DocumentEncoding;
                set { AssertIsNotBuilt(); config.DocumentEncoding = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// <para>
            /// Gets or sets an optional document provider to be used for reading/writing documents.  If omitted/null then
            /// a document provider implementation will be selected automatically.
            /// </para>
            /// </summary>
            /// <value>The document provider.</value>
            public IReadsAndWritesDocument DocumentProvider
            {
                get => config.DocumentProvider;
                set { AssertIsNotBuilt(); config.DocumentProvider = value; }
            }

            /// <summary>
            /// Gets or sets a value which indicates whether the XML document declaration should be omitted when
            /// writing XML documents.  Has no effect unless an XML-based document provider is used.
            /// </summary>
            /// <value><c>true</c> if the XML document declaration should be omitted; otherwise, <c>false</c>.</value>
            public bool OmitXmlDeclaration
            {
                get => config.OmitXmlDeclaration;
                set { AssertIsNotBuilt(); config.OmitXmlDeclaration = value; }
            }

            /// <summary>
            /// Gets or sets a service which provides the default/built-in root contexts for expression resolution.
            /// If null then a default service will be used for this purpose.
            /// </summary>
            /// <value>The built-in contexts provider.</value>
            public Func<ExpressionContext, IGetsNamedTalesValue> BuiltinContextsProvider
            {
                get => config.BuiltinContextsProvider;
                set { AssertIsNotBuilt(); config.BuiltinContextsProvider = value; }
            }

            /// <summary>
            /// Gets or sets a collection of "keyword options" which have been provided to the rendering process externally.
            /// </summary>
            /// <value>The keyword options collection.</value>
            public IDictionary<string, object> KeywordOptions
            {
                get => keywordOptions;
                set { AssertIsNotBuilt(); keywordOptions = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// Gets or sets a value which indicates whether or not source annotation should be written to the rendered document.
            /// Source annotation adds comments to the output indicating the source file information.
            /// </summary>
            /// <value><c>true</c> if source annotation should be included in the output; otherwise, <c>false</c>.</value>
            public bool IncludeSourceAnnotation
            {
                get => config.IncludeSourceAnnotation;
                set { AssertIsNotBuilt(); config.IncludeSourceAnnotation = value; }
            }

            /// <summary>
            /// Gets or sets an action which is used to build &amp; add values to the root ZPT context.
            /// </summary>
            /// <value>The context builder.</value>
            public Action<IConfiguresRootContext, IServiceProvider> ContextBuilder
            {
                get => config.ContextBuilder;
                set { AssertIsNotBuilt(); config.ContextBuilder = value ?? throw new ArgumentNullException(nameof(value)); }
            }

            /// <summary>
            /// <para>
            /// Gets or sets a file system path which is used as the 'base' or root path for template files.
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
            public string SourceAnnotationBasePath
            {
                get => config.SourceAnnotationBasePath;
                set { AssertIsNotBuilt(); config.SourceAnnotationBasePath = value; }
            }

            /// <summary>
            /// Returns an immutable configuration object.  This method may be used only once per instance of <see cref="Builder"/>.
            /// Once it has been used, the configuration is finalised.
            /// </summary>
            /// <returns>The configuration object.</returns>
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
