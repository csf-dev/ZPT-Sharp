using System;
using System.Text;
using System.Collections.Generic;
using ZptSharp.Dom;
using ZptSharp.Expressions;

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

            /// <summary>
            /// Gets or sets an object which provides service-resolution/dependency injection for ZPT Sharp types.
            /// If this is unset or <see langword="null"/> then a default resolution service will be used.
            /// </summary>
            /// <value>The service provider.</value>
            public IServiceProvider ServiceProvider
            {
                get => config.ServiceProvider;
                set { AssertIsNotBuilt(); config.ServiceProvider = value; }
            }

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
            /// Gets or sets the document provider to be used for reading/writing documents.
            /// </para>
            /// <para>
            /// This property is not used if the <see cref="ServiceProvider"/> has been set to anything
            /// other than the default (<see langword="null"/>) value.  If a custom service provider is
            /// used then the document provider must be resolvable from that service provider.
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
                set => config.OmitXmlDeclaration = value;
            }

            /// <summary>
            /// Gets or sets a service which provides the default/built-in root contexts for expression resolution.
            /// If null then a default service will be used for this purpose.
            /// </summary>
            /// <value>The built-in contexts provider.</value>
            public Func<ExpressionContext, IGetsNamedTalesValue> BuiltinContextsProvider
            {
                get => config.BuiltinContextsProvider;
                set => config.BuiltinContextsProvider = value;
            }

            /// <summary>
            /// Gets or sets a collection of "keyword options" which have been provided to the rendering process externally.
            /// </summary>
            /// <value>The keyword options collection.</value>
            public IDictionary<string, object> KeywordOptions
            {
                get => config.KeywordOptions;
                set => config.KeywordOptions = value;
            }

            /// <summary>
            /// Gets or sets a value which indicates whether or not source annotation should be written to the rendered document.
            /// Source annotation adds comments to the output indicating the source file information.
            /// </summary>
            /// <value><c>true</c> if source annotation should be included in the output; otherwise, <c>false</c>.</value>
            public bool IncludeSourceAnnotation
            {
                get => config.IncludeSourceAnnotation;
                set => config.IncludeSourceAnnotation = value;
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
            public Builder()
            {
                config = new RenderingConfig
                {
                    DocumentEncoding = Encoding.UTF8,
                };
            }
        }
    }
}
