using System;
using System.Xml;
using Microsoft.Extensions.DependencyInjection;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IGetsXmlReaderSettings"/> which gets a resolver that will
    /// parse DTDs and which uses a local set of XHTML DTDs.
    /// </summary>
    public class XmlReaderSettingsFactory : IGetsXmlReaderSettings
    {
        readonly IGetsXhtmlDtds dtdProvider;
        readonly IServiceProvider provider;

        Config.RenderingConfig Config => provider.GetRequiredService<Config.RenderingConfig>();

        /// <summary>
        /// Gets the reader settings.
        /// </summary>
        /// <returns>The reader settings.</returns>
        public XmlReaderSettings GetReaderSettings()
        {
            return new XmlReaderSettings
            {
                XmlResolver = Resolver,
                DtdProcessing = DtdProcessing.Parse,
            };
        }

        /// <summary>
        /// Gets the XML resolver which will be returned within the output of <see cref="GetReaderSettings"/>.
        /// </summary>
        public XmlResolver Resolver => Config.XmlUrlResolver ?? new XhtmlOnlyXmlUrlResolver(dtdProvider);

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReaderSettingsFactory"/> class.
        /// </summary>
        /// <param name="dtdProvider">A DTD provider service.</param>
        /// <param name="provider">The service provider.</param>
        public XmlReaderSettingsFactory(IGetsXhtmlDtds dtdProvider, IServiceProvider provider)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            this.dtdProvider = dtdProvider ?? throw new ArgumentNullException(nameof(dtdProvider));
        }
    }
}
