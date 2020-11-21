using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class XmlElementTests
    {
        #region ToString

        [Test]
        public void ToString_returns_HTML_open_tag()
        {
            var xml = @"<div xmlns:tal=""http://xml.zope.org/namespaces/tal"" class=""foo"" tal:repeat=""item items"" />";
            Assert.That(() => XmlDocumentUtil.GetNode(xml).ToString(),
                        Is.EqualTo(@"<div {http://www.w3.org/2000/xmlns/}tal=""http://xml.zope.org/namespaces/tal"" class=""foo"" {http://xml.zope.org/namespaces/tal}repeat=""item items"">"));
        }

        #endregion

        #region IsInNamespace

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_no_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div xmlns:tal=""http://xml.zope.org/namespaces/tal"" class=""foo"" tal:repeat=""item items"" />";
            Assert.That(() => XmlDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_when_element_has_matching_namespace_URI(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:div xmlns:tal=""http://xml.zope.org/namespaces/tal"" class=""foo"" tal:repeat=""item items"" />";
            Assert.That(() => XmlDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_when_element_has_matching_namespace_URI_with_different_alias(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<foo:div xmlns:foo=""http://xml.zope.org/namespaces/tal"" class=""foo"" foo:repeat=""item items"" />";
            Assert.That(() => XmlDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_different_namespace_URI(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<metal:div xmlns:tal=""http://xml.zope.org/namespaces/tal"" xmlns:metal=""http://xml.zope.org/namespaces/metal"" class=""foo"" tal:repeat=""item items"" />";
            Assert.That(() => XmlDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_element_has_different_namespace_URI_with_same_alias(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:div xmlns:tal=""http://xml.zope.org/namespaces/metal"" class=""foo"" tal:repeat=""item items"" />";
            Assert.That(() => XmlDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        #endregion

        #region ChildNodes

        [Test]
        public void Adding_a_child_node_modifies_native_document()
        {
            var html1 = @"<div></div>";
            var element1 = XmlDocumentUtil.GetNode(html1);

            var html2 = @"<p>Foo bar</p>";
            var element2 = XmlDocumentUtil.GetNode(html2);

            element1.ChildNodes.Add(element2);
            Assert.That(element1.NativeElement.ToString(), Is.EqualTo(@"<div>
  <p>Foo bar</p>
</div>"));
        }

        [Test]
        public void Removing_a_child_node_modifies_native_document()
        {
            var html = @"<div><p>Foo bar</p></div>";
            var element = XmlDocumentUtil.GetNode(html);

            element.ChildNodes.RemoveAt(0);
            Assert.That(element.NativeElement.ToString(), Is.EqualTo("<div />"));
        }

        #endregion

        #region Attributes

        [Test]
        public void Adding_an_attribute_modifies_native_document()
        {
            var html = @"<div></div>";
            var element = XmlDocumentUtil.GetNode(html);

            var native = new XAttribute("foo", "bar");
            element.Attributes.Add(new XmlAttribute(native, element));
            Assert.That(element.NativeElement.ToString(), Is.EqualTo(@"<div foo=""bar""></div>"));
        }

        [Test]
        public void Removing_an_attribute_node_modifies_native_document()
        {
            var html = @"<div foo=""bar""></div>";
            var element = XmlDocumentUtil.GetNode(html);

            element.Attributes.RemoveAt(0);
            Assert.That(element.NativeElement.ToString(), Is.EqualTo("<div></div>"));
        }

        #endregion

        #region GetCopy

        [Test, AutoMoqData]
        public void GetCopy_returns_deep_copy_of_selected_node()
        {
            var html = @"<html><body><div class=""foo""><p id=""test"">Hello there</p><p>Another paragraph</p></div></body></html>";
            var xmlElement = XmlDocumentUtil.GetNode(html);
            var bodyElement = xmlElement.ChildNodes.First();
            var result = (XmlElement) bodyElement.GetCopy();

            Assert.That(result.NativeElement.ToString(), Is.EqualTo(@"<body>
  <div class=""foo"">
    <p id=""test"">Hello there</p>
    <p>Another paragraph</p>
  </div>
</body>"));
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
            var xmlElement = XmlDocumentUtil.GetNode(html);
            var bodyElement = xmlElement.ChildNodes.Skip(1).First();

            var result = bodyElement.GetCopy();

            var testElement = result.ChildNodes.Skip(1).First().ChildNodes.Skip(1).First();
            Assert.That(testElement.SourceInfo?.StartTagLineNumber, Is.EqualTo(4));
        }

        #endregion
    }
}
