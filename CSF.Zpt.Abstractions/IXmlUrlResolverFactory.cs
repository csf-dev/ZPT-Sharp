using System;
using System.Xml;

namespace CSF.Zpt
{
  public interface IXmlUrlResolverFactory
  {
    XmlUrlResolver GetResolver();
  }
}

