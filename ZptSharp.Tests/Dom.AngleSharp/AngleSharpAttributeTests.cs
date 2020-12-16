using System;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class AngleSharpAttributeTests
    {
        #region Matches

        [Test, AutoMoqData]
        public void Matches_returns_true_if_attribute_has_same_name_and_prefix(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div tal:repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.True);
        }

        [Test, AutoMoqData]
        public void Matches_returns_true_if_element_has_same_prefix_and_attriute_has_right_name(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<tal:block repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.True);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_name(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div tal:foo=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_prefix(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div foo:repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        #endregion

        #region IsInNamespace

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_if_attribute_has_same_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div tal:repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_if_element_has_same_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:block repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_if_attribute_has_different_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div foo:repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_if_attribute_has_no_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        #endregion

        #region IsNamespaceDeclarationFor

        [Test, AutoMoqData]
        public void IsNamespaceDeclarationFor_returns_false(string name, Namespace @namespace)
        {
            var html = @"<div repeat=""item items"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attrib = element.Attributes.Single();
            Assert.That(() => attrib.IsNamespaceDeclarationFor(@namespace), Is.False);
        }

        #endregion

        #region Value

        [Test, AutoMoqData]
        public void Value_returns_HTML_decoded_result_for_encoded_source()
        {
            var html = @"<div repeat=""item string:&quot;This is &gt; than that!&quot;"">";
            var element = AngleSharpDocumentUtil.GetNode(html);
            var attrib = element.Attributes.Single();
            Assert.That(() => attrib.Value, Is.EqualTo("item string:\"This is > than that!\""));
        }

        [Test, AutoMoqData]
        public void Value_can_be_set_using_HTML_entities()
        {
            var html = @"<div repeat=""item string:&quot;This is &gt; than that!&quot;"">";
            var doc = AngleSharpDocumentUtil.GetDocument(html);
            var entity = AngleSharpDocumentUtil.GetElementsFromFragmentBasedDocument(doc).First();
            entity.Attributes.Single().Value = "\"Foo > Bar < Baz\"";

            Assert.That(() => AngleSharpDocumentUtil.GetRendering(doc).Result,
                        Is.EqualTo(@"<html><head></head><body><div repeat=""&quot;Foo > Bar < Baz&quot;""></div></body></html>"));
        }

        #endregion
    }
}
