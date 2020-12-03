using System;
using NUnit.Framework;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class VariableDefinitionTests
    {
        [Test, AutoMoqData]
        public void Equals_returns_false_if_other_is_null(VariableDefinition sut)
        {
            Assert.That(() => sut.Equals(null), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_other_is_a_different_object_type(VariableDefinition sut, object other)
        {
            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_false_if_other_has_different_variable_name(VariableDefinition sut, VariableDefinition other)
        {
            sut.VariableName = "foo";
            other.VariableName = "bar";

            Assert.That(() => sut.Equals(other), Is.False);
        }

        [Test, AutoMoqData]
        public void Equals_returns_true_if_other_has_same_variable_name_expression_and_scope(VariableDefinition sut,
                                                                                             VariableDefinition other,
                                                                                             string name,
                                                                                             string expression)
        {
            sut.VariableName = other.VariableName = name;
            sut.Expression = other.Expression = expression;
            sut.Scope = other.Scope = VariableDefinition.LocalScope;

            Assert.That(() => sut.Equals(other), Is.True);
        }

        [Test, AutoMoqData]
        public void GetHashCode_returns_zero(VariableDefinition sut)
        {
            Assert.That(() => sut.GetHashCode(), Is.Zero);
        }

    }
}
