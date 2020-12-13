using System;
using System.Collections.Generic;
using System.Xml;

namespace ZptSharp.Dom
{
    /// <summary>
    /// A specialisation of <see cref="XmlUrlResolver"/> which resolves only
    /// XHTML 1.0 and XHTML 1.1 entities.  This class does not permit the direct usage
    /// of network requests, it makes use of a <see cref="IGetsXhtmlDtds"/> as its source
    /// for the DTD/entity/module resources.
    /// </summary>
    public class XhtmlOnlyXmlUrlResolver : XmlUrlResolver
    {
        const string
            Xhtml1StrictRelative            = "-//W3C//DTD XHTML 1.0 Strict//EN",
            Xhtml1TransitionalRelative      = "-//W3C XHTML 1.0 Transitional//EN",
            Xhtml1TransitionalDtdRelative   = "-//W3C//DTD XHTML 1.0 Transitional//EN",
            Xhtml1FramesetRelative          = "-//W3C XHTML 1.0 Frameset//EN",
            Xhtml11Relative                 = "-//W3C//DTD XHTML 1.1//EN";

        /// <summary>
        /// Gets a Uri for the root of the w3.org's resolution namespace.
        /// </summary>
        public static readonly Uri W3BaseUri = new Uri("http://www.w3.org/");

        /// <summary>
        /// Gets a Uri for the root location where XHTML modules are found.
        /// </summary>
        public static readonly Uri ModuleBaseUri = new Uri(W3BaseUri, "MarkUp/DTD/");

        /// <summary>
        /// Gets a Uri for the root location where XHTML DTDs are found.
        /// </summary>
        public static readonly Uri DtdBaseUri = new Uri(W3BaseUri, "TR/xhtml1/DTD/");

        static readonly Dictionary<string, Uri> xhtmlUrisByRelativeUri = new Dictionary<string, Uri> {
            { Xhtml1StrictRelative,             new Uri(DtdBaseUri, "xhtml1-strict.dtd") },
            { Xhtml1TransitionalRelative,       new Uri(DtdBaseUri, "xhtml1-transitional.dtd") },
            { Xhtml1TransitionalDtdRelative,    new Uri(DtdBaseUri, "xhtml1-transitional.dtd") },
            { Xhtml1FramesetRelative,           new Uri(DtdBaseUri, "xhtml1-frameset.dtd") },
            { Xhtml11Relative,                  new Uri(DtdBaseUri, "xhtml11.dtd") }
        };

        readonly IGetsXhtmlDtds xhtmlDtdProvider;

        /// <summary>
        /// Resolves the URI from a base URI and a relative URI component.
        /// </summary>
        /// <returns>The resolved URI.</returns>
        /// <param name="baseUri">Base URI.</param>
        /// <param name="relativeUri">Relative URI.</param>
        public override Uri ResolveUri(Uri baseUri, string relativeUri)
        {
            if (xhtmlUrisByRelativeUri.ContainsKey(relativeUri))
                return xhtmlUrisByRelativeUri[relativeUri];

            return base.ResolveUri(baseUri, relativeUri);
        }

        /// <summary>
        /// Gets the requested entity from a URI.
        /// </summary>
        /// <returns>The entity.</returns>
        /// <param name="absoluteUri">Absolute URI.</param>
        /// <param name="role">Role.</param>
        /// <param name="ofObjectToReturn">Of object to return.</param>
        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));

            if (ModuleBaseUri.IsBaseOf(absoluteUri)
             || DtdBaseUri.IsBaseOf(absoluteUri))
            {
                return xhtmlDtdProvider.GetXhtmlDtdStream(absoluteUri);
            }

            var message = String.Format(Resources.ExceptionMessages.UnsupportedEntity,
                                        GetType().Name,
                                        nameof(XmlResolver),
                                        nameof(Config.RenderingConfig));
            throw new NotSupportedException(message);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XhtmlOnlyXmlUrlResolver"/> class.
        /// </summary>
        /// <param name="xhtmlDtdProvider">Xhtml dtd provider.</param>
        public XhtmlOnlyXmlUrlResolver(IGetsXhtmlDtds xhtmlDtdProvider)
        {
            this.xhtmlDtdProvider = xhtmlDtdProvider ?? throw new ArgumentNullException(nameof(xhtmlDtdProvider));
        }
    }
}
