using System;
using System.Xml;

namespace CSF.Zpt
{
  /// <summary>
  /// Default implementation of <see cref="IXmlUrlResolverFactory"/>.
  /// </summary>
  public class XmlUrlResolverFactory : IXmlUrlResolverFactory
  {
    #region fields

    private static readonly XmlUrlResolver _default;

    #endregion

    #region methods

    /// <summary>
    /// Gets the XML URL resolver.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This implementation returns a singleton of a <see cref="LocalXhtmlXmlResolver"/>.  This type contains the
    /// common XHTML DTDs and entity definitions, baked into the assembly.  This means that for those document-types,
    /// no HTTP request is required for doctype validation.
    /// </para>
    /// </remarks>
    /// <returns>The resolver.</returns>
    public XmlUrlResolver GetResolver()
    {
      return _default;
    }

    #endregion

    #region constructor

    static XmlUrlResolverFactory()
    {
      _default = new LocalXhtmlXmlResolver();
    }

    #endregion
  }
}

