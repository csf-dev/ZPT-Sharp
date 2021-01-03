using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace ZptSharp.Expressions.CSharpExpressions
{
    [TestFixture, Parallelizable]
    public class ExpressionDescriptionFactoryTests
    {
        [Test,AutoMoqData]
        public void GetIdentity_returns_identity_with_correct_properties(ExpressionDescriptionFactory sut,
                                                                         string expressionBody,
                                                                         AssemblyReference ref1,
                                                                         AssemblyReference ref2,
                                                                         UsingNamespace using1,
                                                                         UsingNamespace using2,
                                                                         string var1,
                                                                         string var2)
        {
            var variables = new Dictionary<string,object>
            {
                { "ref1_name", ref1 },
                { "ref2_name", ref2 },
                { "using1_name", using1 },
                { "using2_name", using2 },
                { "var1_name", var1 },
                { "var2_name", var2 },
            };
            var expected = new ExpressionDescription(expressionBody,
                                                     new[] { ref1, ref2 },
                                                     new[] { using1, using2 },
                                                     variables.Keys,
                                                     Enumerable.Empty<VariableType>());


            var result = sut.GetDescription(expressionBody, variables);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}