using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class XmlAttributeTests
    {
        #region Matches

        [Test, AutoMoqData]
        public void Matches_returns_true_if_attribute_has_same_name_and_namespace_uri(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div xmlns:tal=""http://xml.zope.org/namespaces/tal"" tal:repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.Matches(spec), Is.True);
        }

        [Test, AutoMoqData]
        public void Matches_returns_true_if_attribute_has_same_name_and_namespace_uri_but_different_alias(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div xmlns:foo=""http://xml.zope.org/namespaces/tal"" foo:repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.Matches(spec), Is.True);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_name(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div xmlns:tal=""http://xml.zope.org/namespaces/tal"" tal:foo=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_namespace_uri(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div xmlns:metal=""http://xml.zope.org/namespaces/metal"" metal:repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_namespace_uri_but_same_alias(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div xmlns:tal=""http://xml.zope.org/namespaces/metal"" tal:repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        #endregion

        #region IsInNamespace

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_if_attribute_has_same_namespace_uri(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div xmlns:tal=""http://xml.zope.org/namespaces/tal"" tal:repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_if_element_same_namespace_uri(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:block xmlns:tal=""http://xml.zope.org/namespaces/tal"" repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_if_attribute_has_different_namespace_uri(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div xmlns:tal=""http://xml.zope.org/namespaces/metal"" tal:repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.Skip(1).First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_if_attribute_has_no_namespace_uri(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div repeat=""item items"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        #endregion

        #region IsNamespaceDeclarationFor

        [Test, AutoMoqData]
        public void IsNamespaceDeclarationFor_returns_true_if_namespace_uri_matches()
        {
            var name = XName.Get("foo", "http://www.w3.org/2000/xmlns/");
            var native = new XAttribute(name, "http://example.com/foo");
            var attrib = new XmlAttribute(native);
            Assert.That(() => attrib.IsNamespaceDeclarationFor(new Namespace("foo", "http://example.com/foo")), Is.True);
        }

        [Test, AutoMoqData]
        public void IsNamespaceDeclarationFor_returns_true_if_prefix_differs_if_namespace_uri_matches()
        {
            var name = XName.Get("foo", "http://www.w3.org/2000/xmlns/");
            var native = new XAttribute(name, "http://example.com/foo");
            var attrib = new XmlAttribute(native);
            Assert.That(() => attrib.IsNamespaceDeclarationFor(new Namespace("bar", "http://example.com/foo")), Is.True);
        }

        [Test, AutoMoqData]
        public void IsNamespaceDeclarationFor_returns_false_if_namespace_uri_differs()
        {
            var name = XName.Get("foo", "http://www.w3.org/2000/xmlns/");
            var native = new XAttribute(name, "http://example.com/foo");
            var attrib = new XmlAttribute(native);
            Assert.That(() => attrib.IsNamespaceDeclarationFor(new Namespace("foo", "http://example.com/bar")), Is.False);
        }

        #endregion

        #region Value

        [Test, AutoMoqData]
        public void Value_returns_HTML_decoded_result_for_encoded_source()
        {
            var html = @"<div repeat=""item string:&quot;This is &gt; than that!&quot;"" />";
            var element = XmlDocumentUtil.GetNode(html);
            var attrib = element.Attributes.Single();
            Assert.That(() => attrib.Value, Is.EqualTo("item string:\"This is > than that!\""));
        }

        [Test, AutoMoqData]
        public void Value_can_be_set_using_HTML_entities()
        {
            var html = @"<div repeat=""item string:&quot;This is &gt; than that!&quot;"" />";
            var doc = XmlDocumentUtil.GetDocument(html);
            var entity = (XmlElement)doc.RootElement;
            entity.Attributes.Single().Value = "\"Foo > Bar < Baz\"";

            Assert.That(() => XmlDocumentUtil.GetRendering(doc),
                        Is.EqualTo(@"<?xml version=""1.0"" encoding=""utf-8""?>
<div repeat=""&quot;Foo &gt; Bar &lt; Baz&quot;"" />"));
        }

        #endregion

    }
}
