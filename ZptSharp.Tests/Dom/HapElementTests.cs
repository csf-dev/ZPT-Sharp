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
        [Test]
        public void ToString_returns_HTML_open_tag()
        {
            var html = @"<div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => GetNode(html).ToString(), Is.EqualTo(html));
        }

        [Test,AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_no_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_when_element_has_matching_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_different_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<metal:div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test]
        public void Adding_a_child_node_modifies_native_document()
        {
            var html1 = @"<div></div>";
            var element1 = GetNode(html1);

            var html2 = @"<p>Foo bar</p>";
            var element2 = GetNode(html2);

            element1.ChildNodes.Add(element2);
            Assert.That(element1.NativeElement.OuterHtml, Is.EqualTo("<div><p>Foo bar</p></div>"));
        }

        [Test]
        public void Removing_a_child_node_modifies_native_document()
        {
            var html = @"<div><p>Foo bar</p></div>";
            var element = GetNode(html);

            element.ChildNodes.RemoveAt(0);
            Assert.That(element.NativeElement.OuterHtml, Is.EqualTo("<div></div>"));
        }

        [Test]
        public void Adding_an_attribute_modifies_native_document()
        {
            var html = @"<div></div>";
            var element = GetNode(html);

            var native = element.NativeElement.OwnerDocument.CreateAttribute("foo", "bar");
            element.Attributes.Add(new HapAttribute(native, element));
            Assert.That(element.NativeElement.OuterHtml, Is.EqualTo(@"<div foo=""bar""></div>"));
        }

        [Test]
        public void Removing_an_attribute_node_modifies_native_document()
        {
            var html = @"<div foo=""bar""></div>";
            var element = GetNode(html);

            element.Attributes.RemoveAt(0);
            Assert.That(element.NativeElement.OuterHtml, Is.EqualTo("<div></div>"));
        }


        HapElement GetNode(string html) => (HapElement) GetDocument(html).RootElement.ChildNodes.First();

        HapDocument GetDocument(string html)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(html)))
            {
                var provider = new HapDocumentProvider();
                return (HapDocument) provider.GetDocumentAsync(stream, RenderingConfig.Default).Result;
            }
        }
    }
}
