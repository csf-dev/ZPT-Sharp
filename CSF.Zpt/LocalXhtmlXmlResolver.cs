using System;
using System.Collections.Generic;
using System.Xml;
using System.Reflection;

namespace CSF.Zpt
{
  internal class LocalXhtmlXmlResolver : XmlUrlResolver
  {
    #region fields

    private static readonly Dictionary<string, Uri> _xhtmlUris = new Dictionary<string, Uri> {
      { "-//W3C//DTD XHTML 1.0 Strict//EN",       new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd") },
      { "-//W3C XHTML 1.0 Transitional//EN",      new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") },
      { "-//W3C//DTD XHTML 1.0 Transitional//EN", new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd") },
      { "-//W3C XHTML 1.0 Frameset//EN",          new Uri("http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd") },
      { "-//W3C//DTD XHTML 1.1//EN",              new Uri("http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd") }
    };

    private static readonly Dictionary<string, string> _xhtmlResourceNames = new Dictionary<string, string> {
      { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd",                      "xhtml1-strict.dtd" },
      { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd",                "xhtml1-transitional.dtd" },
      { "http://www.w3.org/TR/xhtml1/DTD/xhtml1-frameset.dtd",                    "xhtml1-frameset.dtd" },
      { "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd",                           "xhtml11.dtd" },
      { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Latin 1 for XHTML//EN", "xhtml-lat1.ent" },
      { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Special for XHTML//EN", "xhtml-special.ent" },
      { "http://www.w3.org/TR/xhtml1/DTD/-//W3C//ENTITIES Symbols for XHTML//EN", "xhtml-symbol.ent" }
    };

    #endregion

    #region overrides

    public override Uri ResolveUri(Uri baseUri, string relativeUri)
    {
      return _xhtmlUris.ContainsKey(relativeUri) ? _xhtmlUris[relativeUri] : base.ResolveUri(baseUri, relativeUri);
    }

    public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
    {
      if(absoluteUri == null)
      {
        throw new ArgumentNullException(nameof(absoluteUri));
      }

      object output;

      if(_xhtmlResourceNames.ContainsKey(absoluteUri.OriginalString))
      {
        Assembly thisAssembly = Assembly.GetExecutingAssembly();
        output = thisAssembly.GetManifestResourceStream(_xhtmlResourceNames[absoluteUri.OriginalString]);
      }
      else
      {
        output = base.GetEntity(absoluteUri, role, ofObjectToReturn);
      }

      return output;
    }

    #endregion
  }
}

