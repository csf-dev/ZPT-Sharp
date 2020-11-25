using System;
using NUnit.Framework;

namespace ZptSharp.Tal
{
    [TestFixture,Parallelizable]
    public class VariableDefinitionProviderTests
    {
        [Test, AutoMoqData]
        public void GetDefinitions_returns_correct_result_for_a_single_local_definition(VariableDefinitionProvider sut)
        {
            var expected = new[]
            {
                new VariableDefinition
                {
                    VariableName = "foo",
                    Expression = "bar",
                    Scope = VariableDefinition.LocalScope,
                },
            };

            Assert.That(() => sut.GetDefinitions("foo bar"), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_returns_correct_result_for_a_pair_of_local_definitions(VariableDefinitionProvider sut)
        {
            var expected = new[]
            {
                new VariableDefinition
                {
                    VariableName = "foo",
                    Expression = "bar",
                    Scope = VariableDefinition.LocalScope,
                },
                new VariableDefinition
                {
                    VariableName = "var",
                    Expression = "path:here/complex/expression",
                    Scope = VariableDefinition.LocalScope,
                },
            };

            Assert.That(() => sut.GetDefinitions(@"foo bar;
var path:here/complex/expression"), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_returns_correct_result_for_definitions_which_include_an_escaped_semicolon(VariableDefinitionProvider sut)
        {
            var expected = new[]
            {
                new VariableDefinition
                {
                    VariableName = "foo",
                    Expression = "bar",
                    Scope = VariableDefinition.LocalScope,
                },
                new VariableDefinition
                {
                    VariableName = "variable",
                    Expression = "string:This is a nice day; how about we go for a walk?",
                    Scope = VariableDefinition.LocalScope,
                },
            };

            Assert.That(() => sut.GetDefinitions(@"foo bar;
variable string:This is a nice day;; how about we go for a walk?"), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_returns_correct_result_for_an_explicit_local_scope(VariableDefinitionProvider sut)
        {
            var expected = new[]
            {
                new VariableDefinition
                {
                    VariableName = "foo",
                    Expression = "bar",
                    Scope = VariableDefinition.LocalScope,
                },
            };

            Assert.That(() => sut.GetDefinitions("local foo bar"), Is.EqualTo(expected));
        }

        [Test, AutoMoqData]
        public void GetDefinitions_returns_correct_result_for_an_explicit_global_scope(VariableDefinitionProvider sut)
        {
            var expected = new[]
            {
                new VariableDefinition
                {
                    VariableName = "foo",
                    Expression = "bar",
                    Scope = VariableDefinition.GlobalScope,
                },
            };

            Assert.That(() => sut.GetDefinitions("global foo bar"), Is.EqualTo(expected));
        }

    }
}
