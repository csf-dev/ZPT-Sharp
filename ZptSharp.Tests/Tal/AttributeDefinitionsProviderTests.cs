using System;
using NUnit.Framework;
using ZptSharp.Dom;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class AttributeDefinitionsProviderTests
    {
        [Test, AutoMoqData]
        public void GetDefinitions_can_get_a_single_definition(AttributeDefinitionsProvider sut, INode node)
        {
            var expected = new[] { new AttributeDefinition { Name = "class", Expression = "foo" } };
            Assert.That(() => sut.GetDefinitions("class foo", node), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_can_get_a_collection_of_definitions_including_escaped_semicolons(AttributeDefinitionsProvider sut, INode node)
        {
            var expected = new[] {
                new AttributeDefinition { Name = "class", Expression = "foo" },
                new AttributeDefinition { Name = "style", Expression = "string:font-weight: bold; text-decoration: underline" },
                new AttributeDefinition { Name = "id", Expression = "one/two/three" },
            };
            var attributeValue = @"class foo;
style string:font-weight: bold;; text-decoration: underline;
id one/two/three";

            Assert.That(() => sut.GetDefinitions(attributeValue, node), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_can_get_a_definition_which_uses_a_prefix(AttributeDefinitionsProvider sut, INode node)
        {
            var expected = new[] { new AttributeDefinition { Prefix = "html", Name = "class", Expression = "foo" } };
            Assert.That(() => sut.GetDefinitions("html:class foo", node), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_throws_is_an_attribute_definition_is_invalid(AttributeDefinitionsProvider sut, INode node)
        {
            Assert.That(() => sut.GetDefinitions("nope", node), Throws.InstanceOf<InvalidTalAttributeException>());
        }
    }
}
