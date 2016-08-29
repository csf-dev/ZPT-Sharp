using System;
using System.Xml;

namespace CSF.Zpt
{
  public class XmlUrlResolverFactory : IXmlUrlResolverFactory
  {
    #region fields

    private static readonly XmlUrlResolver _default;

    #endregion

    #region methods

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

