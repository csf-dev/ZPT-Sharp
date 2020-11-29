using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class HapElementTests
    {
        #region ToString

        [Test]
        public void ToString_returns_HTML_open_tag()
        {
            var html = @"<div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => HapDocumentUtil.GetNode(html).ToString(), Is.EqualTo(html));
        }

        #endregion

        #region IsInNamespace

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_no_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => HapDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_when_element_has_matching_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => HapDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_different_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<metal:div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => HapDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        #endregion

        #region ChildNodes

        [Test]
        public void Adding_a_child_node_modifies_native_document()
        {
            var html1 = @"<div></div>";
            var element1 = HapDocumentUtil.GetNode(html1);

            var html2 = @"<p>Foo bar</p>";
            var element2 = HapDocumentUtil.GetNode(html2);

            element1.ChildNodes.Add(element2);
            Assert.That(element1.NativeElement.OuterHtml, Is.EqualTo("<div><p>Foo bar</p></div>"));
        }

        [Test]
        public void Removing_a_child_node_modifies_native_document()
        {
            var html = @"<div><p>Foo bar</p></div>";
            var element = HapDocumentUtil.GetNode(html);

            element.ChildNodes.RemoveAt(0);
            Assert.That(element.NativeElement.OuterHtml, Is.EqualTo("<div></div>"));
        }

        #endregion

        #region Attributes

        [Test]
        public void Adding_an_attribute_modifies_native_document()
        {
            var html = @"<div></div>";
            var element = HapDocumentUtil.GetNode(html);

            var native = element.NativeElement.OwnerDocument.CreateAttribute("foo", "bar");
            element.Attributes.Add(new HapAttribute(native) { Element = element });
            Assert.That(element.NativeElement.OuterHtml, Is.EqualTo(@"<div foo=""bar""></div>"));
        }

        [Test]
        public void Removing_an_attribute_node_modifies_native_document()
        {
            var html = @"<div foo=""bar""></div>";
            var element = HapDocumentUtil.GetNode(html);

            element.Attributes.RemoveAt(0);
            Assert.That(element.NativeElement.OuterHtml, Is.EqualTo("<div></div>"));
        }

        #endregion

        #region GetCopy

        [Test, AutoMoqData]
        public void GetCopy_returns_deep_copy_of_selected_node()
        {
            var html = @"<html><body><div class=""foo""><p id=""test"">Hello there</p><p>Another paragraph</p></div></body></html>";
            var htmlElement = HapDocumentUtil.GetNode(html);
            var bodyElement = htmlElement.ChildNodes.First();
            var result = (HapElement) bodyElement.GetCopy();

            Assert.That(result.NativeElement.OuterHtml, Is.EqualTo(@"<body><div class=""foo""><p id=""test"">Hello there</p><p>Another paragraph</p></div></body>"));
        }

        [Test, AutoMoqData]
        public void GetCopy_deep_copies_source_info_line_numbers()
        {
            var html = @"<html>
<body>
    <div class=""foo"">
        <p id=""test"">Hello there</p>
        <p>Another paragraph</p>
    </div>
</body>
</html>";
            var htmlElement = HapDocumentUtil.GetNode(html);
            var bodyElement = htmlElement.ChildNodes.Skip(1).First();

            var result = bodyElement.GetCopy();

            var testElement = result.ChildNodes.Skip(1).First().ChildNodes.Skip(1).First();
            Assert.That(testElement.SourceInfo?.StartTagLineNumber, Is.EqualTo(4));
        }

        #endregion
    }
}
