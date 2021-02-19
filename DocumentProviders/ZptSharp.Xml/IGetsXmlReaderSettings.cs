using System;
using System.Xml;

namespace ZptSharp.Dom
{
    /// <summary>
    /// An object which gets the <see cref="XmlReaderSettings"/> for reading XML documents in ZptSharp.
    /// </summary>
    public interface IGetsXmlReaderSettings
    {
        /// <summary>
        /// Gets the reader settings.
        /// </summary>
        /// <returns>The reader settings.</returns>
        XmlReaderSettings GetReaderSettings();
    }
}
