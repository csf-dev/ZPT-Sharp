using System.Threading.Tasks;
using NUnit.Framework;

namespace ZptSharp.Expressions.CSharpExpressions
{
    [TestFixture, Parallelizable]
    public class VariableTypeEvaluatorTests
    {
        [Test,AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_object_with_correct_properties_for_valid_expression(VariableTypeEvaluator sut,
                                                                                                              ExpressionContext context)
        {
            var result = await sut.EvaluateExpressionAsync("one int[]", context);
            Assert.That(result,
                        Has.Property(nameof(VariableType.VariableName)).EqualTo("one")
                            .And.Property(nameof(VariableType.Type)).EqualTo("int[]"));
        }

        [Test,AutoMoqData]
        public void EvaluateExpressionAsync_throws_for_invalid_expression(VariableTypeEvaluator sut,
                                                                                ExpressionContext context)
        {
            Assert.That(async () => await sut.EvaluateExpressionAsync("nope", context),
                        Throws.InstanceOf<CSharpEvaluationException>()
                            .And.Message.StartsWith("The C# 'type' expression must be syntactically valid"));
        }
    }
}