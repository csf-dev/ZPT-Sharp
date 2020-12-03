using System;
using NUnit.Framework;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class AttributeDefinitionTests
    {
        [Test, AutoMoqData]
        public void Equals_returns_false_for_null(AttributeDefinition sut)
        {
            Assert.That(() => sut.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_a_different_object_type(AttributeDefinition sut, object other)
        {
            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_same_object(AttributeDefinition sut)
        {
#pragma warning disable RECS0088 // Comparing equal expression for equality is usually useless
            Assert.That(() => sut.Equals(sut), Is.True);
#pragma warning restore RECS0088 // Comparing equal expression for equality is usually useless
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_for_definition_with_same_prefix_name_and_expression(AttributeDefinition sut,
                                                                                            AttributeDefinition other)
        {
            other.Prefix = sut.Prefix;
            other.Name = sut.Name;
            other.Expression = sut.Expression;

            Assert.That(() => sut.Equals(other), Is.True);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_for_definition_with_different_name(AttributeDefinition sut,
                                                                            AttributeDefinition other)
        {
            other.Prefix = sut.Prefix;
            other.Name = "foo";
            sut.Name = "bar";
            other.Expression = sut.Expression;

            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_zero(AttributeDefinition sut)
        {
            Assert.That(() => sut.GetHashCode(), Is.Zero);
        }

        [Test, AutoMoqData]
        public void ToString_returns_name_and_expression_if_it_has_no_prefix(AttributeDefinition sut)
        {
            sut.Name = "class";
            sut.Expression = "foo bar baz";
            sut.Prefix = null;

            Assert.That(() => sut.ToString(), Is.EqualTo("class=\"foo bar baz\""));
        }

        [Test, AutoMoqData]
        public void ToString_returns_name_and_expression_if_it_has_empty_prefix(AttributeDefinition sut)
        {
            sut.Name = "class";
            sut.Expression = "foo bar baz";
            sut.Prefix = String.Empty;

            Assert.That(() => sut.ToString(), Is.EqualTo("class=\"foo bar baz\""));
        }

        [Test, AutoMoqData]
        public void ToString_returns_prefix_name_and_expression_if_it_has_prefix(AttributeDefinition sut)
        {
            sut.Name = "class";
            sut.Expression = "foo bar baz";
            sut.Prefix = "html";

            Assert.That(() => sut.ToString(), Is.EqualTo("html:class=\"foo bar baz\""));
        }
    }
}
