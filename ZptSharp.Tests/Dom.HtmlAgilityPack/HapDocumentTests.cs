using System;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using ZptSharp.Rendering;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class HapDocumentTests
    {
        [Test]
        public void AddCommentToBeginningOfDocument_adds_the_comment()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = GetSut(html);

            sut.AddCommentToBeginningOfDocument("This is a comment");

            Assert.That(GetHtml(sut.NativeDocument), Is.EqualTo("<!--This is a comment--><html><body><div>Hello</div></body></html>"));
        }

        HapDocument GetSut(string html)
        {
            var native = new HtmlDocument();
            native.LoadHtml(html);
            return new HapDocument(native, new UnknownSourceInfo());
        }

        string GetHtml(HtmlDocument native)
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
