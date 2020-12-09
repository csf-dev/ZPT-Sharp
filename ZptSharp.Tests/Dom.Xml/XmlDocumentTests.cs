using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class XmlDocumentTests
    {
        [Test]
        public void AddCommentToBeginningOfDocument_adds_the_comment()
        {
            var xml = "<html><body><div>Hello</div></body></html>";
            var sut = GetSut(xml);

            sut.AddCommentToBeginningOfDocument("This is a comment");

            Assert.That(GetXml(sut.NativeDocument), Is.EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<!--This is a comment-->
<html>
  <body>
    <div>Hello</div>
  </body>
</html>"));
        }

        [Test]
        public void ReplaceRootElement_can_replace_the_root_element_in_the_doc()
        {
            var xml = "<html><body><div>Hello</div></body></html>";
            var sut = GetSut(xml);
            var replacement = sut.RootElement.ChildNodes.First();

            sut.ReplaceRootElement(replacement);

            Assert.That(GetXml(sut.NativeDocument), Is.EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<body>
  <div>Hello</div>
</body>"));
        }

        XmlDocument GetSut(string xml)
        {
            var native = XDocument.Parse(xml);
            return new XmlDocument(native, new UnknownSourceInfo());
        }

        string GetXml(XDocument native)
        {
            using (var stream = new MemoryStream())
            {
                native.Save(stream);

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
        }
    }
}
