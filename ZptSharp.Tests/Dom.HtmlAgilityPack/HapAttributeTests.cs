using System;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class HapAttributeTests
    {
        #region Matches

        [Test, AutoMoqData]
        public void Matches_returns_true_if_attribute_has_same_name_and_prefix(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div tal:repeat=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.True);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_name(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div tal:foo=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        [Test, AutoMoqData]
        public void Matches_returns_false_if_attribute_has_different_prefix(WellKnownNamespaceProvider namespaces)
        {
            var spec = new AttributeSpec("repeat", namespaces.TalNamespace);
            var html = @"<div foo:repeat=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.Matches(spec), Is.False);
        }

        #endregion

        #region IsInNamespace

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_if_attribute_has_same_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div tal:repeat=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_if_attribute_has_different_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div foo:repeat=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_if_attribute_has_no_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div repeat=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attribute = element.Attributes.First();

            Assert.That(() => attribute.IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        #endregion

        #region IsNamespaceDeclarationFor

        [Test, AutoMoqData]
        public void IsNamespaceDeclarationFor_returns_false(string name, Namespace @namespace)
        {
            var html = @"<div repeat=""item items"">";
            var element = HapDocumentUtil.GetNode(html);
            var attrib = element.Attributes.Single();
            Assert.That(() => attrib.IsNamespaceDeclarationFor(@namespace), Is.False);
        }

        #endregion

    }
}
