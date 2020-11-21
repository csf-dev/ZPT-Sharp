using System;
using System.Linq;
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
    }
}
