using System;
using System.IO;
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
        public void CreateComment_throws_ane_if_content_is_null()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = GetSut(html);

            Assert.That(() => sut.CreateComment(null), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void CreateComment_returns_a_comment(string commentText)
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = GetSut(html);

            Assert.That(() => sut.CreateComment(commentText), Is.Not.Null);
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
