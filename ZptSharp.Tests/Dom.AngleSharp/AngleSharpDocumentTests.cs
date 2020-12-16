using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ZptSharp.Rendering;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AngleSharpDocumentTests
    {
        [Test]
        public void AddCommentToBeginningOfDocument_adds_the_comment()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = GetSut(html);

            sut.AddCommentToBeginningOfDocument("This is a comment");

            Assert.That(GetHtml(sut.NativeDocument), Is.EqualTo("<!--This is a comment--><html><body><div>Hello</div></body></html>"));
        }

        AngleSharpDocument GetSut(string html)
        {
            var context = BrowsingContext.New();
            var doc = (IHtmlDocument) context.OpenAsync(req => req.Content(html)).Result;
            return new AngleSharpDocument(doc, new UnknownSourceInfo());
        }

        string GetHtml(IHtmlDocument native) => native.DocumentElement.OuterHtml;
    }
}
