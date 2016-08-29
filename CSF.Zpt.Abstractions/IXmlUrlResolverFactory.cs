using System;
using System.Xml;

namespace CSF.Zpt
{
  /// <summary>
  /// Interface for a service which can return instances of <c>XmlUrlResolver</c>.
  /// </summary>
  public interface IXmlUrlResolverFactory
  {
    /// <summary>
    /// Gets the XML URL resolver.
    /// </summary>
    /// <returns>The resolver.</returns>
    XmlUrlResolver GetResolver();
  }
}

