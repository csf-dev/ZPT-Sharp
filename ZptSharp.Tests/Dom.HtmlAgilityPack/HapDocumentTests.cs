using System;
using System.IO;
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

        HapDocument GetSut(string html)
        {
            var native = new HtmlAgilityPack.HtmlDocument();
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
