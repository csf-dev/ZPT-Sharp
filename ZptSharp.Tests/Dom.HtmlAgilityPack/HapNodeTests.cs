﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ZptSharp.Config;

namespace ZptSharp.Dom
{
    [TestFixture,Parallelizable]
    public class HapNodeTests
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
        public void IsInNamespace_returns_false_when_node_has_no_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => HapDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.False);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_true_when_node_has_matching_namespace_prefix(WellKnownNamespaceProvider namespaces)
        {
            var html = @"<tal:div class=""foo"" tal:repeat=""item items"">";
            Assert.That(() => HapDocumentUtil.GetNode(html).IsInNamespace(namespaces.TalNamespace), Is.True);
        }

        [Test, AutoMoqData]
        public void IsInNamespace_returns_false_when_node_has_different_namespace_prefix(WellKnownNamespaceProvider namespaces)
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
            var node1 = HapDocumentUtil.GetNode(html1);

            var html2 = @"<p>Foo bar</p>";
            var node2 = HapDocumentUtil.GetNode(html2);

            node1.ChildNodes.Add(node2);
            Assert.That(node1.NativeNode.OuterHtml, Is.EqualTo("<div><p>Foo bar</p></div>"));
        }

        [Test]
        public void Removing_a_child_node_modifies_native_document()
        {
            var html = @"<div><p>Foo bar</p></div>";
            var node = HapDocumentUtil.GetNode(html);

            node.ChildNodes.RemoveAt(0);
            Assert.That(node.NativeNode.OuterHtml, Is.EqualTo("<div></div>"));
        }

        #endregion

        #region Attributes

        [Test]
        public void Adding_an_attribute_modifies_native_document()
        {
            var html = @"<div></div>";
            var node = HapDocumentUtil.GetNode(html);

            var native = node.NativeNode.OwnerDocument.CreateAttribute("foo", "bar");
            node.Attributes.Add(new HapAttribute(native) { Node = node });
            Assert.That(node.NativeNode.OuterHtml, Is.EqualTo(@"<div foo=""bar""></div>"));
        }

        [Test]
        public void Removing_an_attribute_node_modifies_native_document()
        {
            var html = @"<div foo=""bar""></div>";
            var node = HapDocumentUtil.GetNode(html);

            node.Attributes.RemoveAt(0);
            Assert.That(node.NativeNode.OuterHtml, Is.EqualTo("<div></div>"));
        }

        #endregion

        #region GetCopy

        [Test, AutoMoqData]
        public void GetCopy_returns_deep_copy_of_selected_node()
        {
            var html = @"<html><body><div class=""foo""><p id=""test"">Hello there</p><p>Another paragraph</p></div></body></html>";
            var htmlNode = HapDocumentUtil.GetNode(html);
            var bodyNode = htmlNode.ChildNodes.First();
            var result = (HapNode) bodyNode.GetCopy();

            Assert.That(result.NativeNode.OuterHtml, Is.EqualTo(@"<body><div class=""foo""><p id=""test"">Hello there</p><p>Another paragraph</p></div></body>"));
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
            var htmlNode = HapDocumentUtil.GetNode(html);
            var bodyNode = htmlNode.ChildNodes.Skip(1).First();

            var result = bodyNode.GetCopy();

            var testNode = result.ChildNodes.Skip(1).First().ChildNodes.Skip(1).First();
            Assert.That(testNode.SourceInfo?.StartTagLineNumber, Is.EqualTo(4));
        }

        #endregion

        #region CreateComment

        [Test]
        public void CreateComment_does_not_throw_if_content_is_null()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = HapDocumentUtil.GetNode(html);

            Assert.That(() => sut.CreateComment(null), Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void CreateComment_returns_a_comment(string commentText)
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = HapDocumentUtil.GetNode(html);

            Assert.That(() => sut.CreateComment(commentText), Is.Not.Null);
        }

        #endregion

        #region CreateTextNode

        [Test, AutoMoqData]
        public void CreateTextNode_returns_a_node_object_which_is_not_an_node(string content)
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = HapDocumentUtil.GetNode(html);

            Assert.That(() => sut.CreateTextNode(content), Is.Not.Null.And.Property(nameof(INode.IsElement)).False);
        }

        #endregion

        #region CreateAttribute

        [Test, AutoMoqData]
        public void CreateAttribute_returns_an_attribute_object_with_correct_name()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = HapDocumentUtil.GetNode(html);
            var spec = new AttributeSpec("class");

            var result = sut.CreateAttribute(spec);

            Assert.That(result?.Name, Is.EqualTo("class"));
        }

        [Test, AutoMoqData]
        public void CreateAttribute_can_create_a_prefixed_attribute_with_namespace()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = HapDocumentUtil.GetNode(html);
            var spec = new AttributeSpec("class", new Namespace("foo"));

            var result = sut.CreateAttribute(spec);

            Assert.That(result?.Name, Is.EqualTo("foo:class"));
        }

        #endregion

        #region ParseAsNodes

        [Test, AutoMoqData]
        public void ParseAsNodes_can_create_an_HTML_structure()
        {
            var html = "<html><body><div>Hello</div></body></html>";
            var sut = HapDocumentUtil.GetNode(html);

            var result = sut.ParseAsNodes("<p><span class=\"test\">Text node</span></p>");

            Assert.That(result.First().ChildNodes.First().Attributes.First().Name, Is.EqualTo("class"));
        }

        #endregion
    }
}
