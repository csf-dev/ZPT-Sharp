using System;
using System.Xml;

namespace ZptSharp.Dom
{
    /// <summary>
    /// Implementation of <see cref="IGetsXmlReaderSettings"/> which gets a resolver that will
    /// parse DTDs and which uses a local set of XHTML DTDs.
    /// </summary>
    public class XmlReaderSettingsFactory : IGetsXmlReaderSettings
    {
        readonly IGetsXhtmlDtds dtdProvider;

        /// <summary>
        /// Gets the reader settings.
        /// </summary>
        /// <returns>The reader settings.</returns>
        public XmlReaderSettings GetReaderSettings()
        {
            var resolver = new XhtmlOnlyXmlUrlResolver(dtdProvider);

            return new XmlReaderSettings
            {
                XmlResolver = resolver,
                DtdProcessing = DtdProcessing.Parse,
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlReaderSettingsFactory"/> class.
        /// </summary>
        /// <param name="dtdProvider">Dtd provider.</param>
        public XmlReaderSettingsFactory(IGetsXhtmlDtds dtdProvider)
        {
            this.dtdProvider = dtdProvider ?? throw new ArgumentNullException(nameof(dtdProvider));
        }
    }
}
