using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ZptSharp.Dom.Resources;

namespace ZptSharp.Dom.Xml
{
    [TestFixture,Parallelizable]
    public class EmbeddedResourceXhtmlDtdProviderTests
    {
        [Test]
        public void GetXhtmlDtdStream_can_get_supported_DTDs([ValueSource(nameof(GetSupportedDtdStrings))] string uriString)
        {
            var sut = new EmbeddedResourceXhtmlDtdProvider();
            var uri = new Uri(uriString);

            Assert.That(() => sut.GetXhtmlDtdStream(uri), Is.Not.Null);
        }

        [Test]
        public void GetXhtmlDtdStream_throws_for_invalid_uri()
        {
            var sut = new EmbeddedResourceXhtmlDtdProvider();
            var uri = new Uri("http://nonsense/uri");

            Assert.That(() => sut.GetXhtmlDtdStream(uri), Throws.ArgumentException);
        }

        public static IEnumerable<string> GetSupportedDtdStrings()
        {
            return new [] {
                new Uri(XhtmlOnlyXmlUrlResolver.DtdBaseUri, "xhtml1-strict.dtd"),
                new Uri(XhtmlOnlyXmlUrlResolver.DtdBaseUri, "xhtml1-transitional.dtd"),
                new Uri(XhtmlOnlyXmlUrlResolver.DtdBaseUri, "xhtml1-transitional.dtd"),
                new Uri(XhtmlOnlyXmlUrlResolver.DtdBaseUri, "xhtml1-frameset.dtd"),
                new Uri(XhtmlOnlyXmlUrlResolver.DtdBaseUri, "xhtml11.dtd"),
                new Uri(XhtmlOnlyXmlUrlResolver.ModuleBaseUri, "xhtml-inlpres.mod"),
            }
            .Select(x => x.OriginalString)
            .ToList();
        }
    }
}
