using System.Linq;
using NUnit.Framework;

namespace ZptSharp.Expressions.CSharpExpressions
{
    [TestFixture, Parallelizable]
    public class ExpressionDescriptionTests
    {
        [Test,AutoMoqData]
        public void Equals_returns_true_for_two_equal_instances(string expression,
                                                                string assembly1,
                                                                string assembly2,
                                                                string using1,
                                                                string using2,
                                                                string var1,
                                                                string var2)
        {
            var first = new ExpressionDescription(expression,
                                                  new[] { new AssemblyReference(assembly1), new AssemblyReference(assembly2) },
                                                  new[] { new UsingNamespace(using1), new UsingNamespace(using2) },
                                                  new[] { var1, var2 },
                                                  Enumerable.Empty<VariableType>());

            var second = new ExpressionDescription(expression,
                                                   new[] { new AssemblyReference(assembly1), new AssemblyReference(assembly2) },
                                                   new[] { new UsingNamespace(using1), new UsingNamespace(using2) },
                                                   new[] { var1, var2 },
                                                   Enumerable.Empty<VariableType>());

            Assert.That(() => first.Equals(second), Is.True);
        }

        [Test,AutoMoqData]
        public void Equals_returns_false_for_two_different_instances(string expression1,
                                                                     string expression2,
                                                                     string assembly1,
                                                                     string assembly2,
                                                                     string assembly3,
                                                                     string assembly4,
                                                                     string using1,
                                                                     string using2,
                                                                     string using3,
                                                                     string using4,
                                                                     string var1,
                                                                     string var2,
                                                                     string var3,
                                                                     string var4)
        {
            var first = new ExpressionDescription(expression1,
                                                  new[] { new AssemblyReference(assembly1), new AssemblyReference(assembly2) },
                                                  new[] { new UsingNamespace(using1), new UsingNamespace(using2) },
                                                  new[] { var1, var2 },
                                                  Enumerable.Empty<VariableType>());

            var second = new ExpressionDescription(expression2,
                                                   new[] { new AssemblyReference(assembly3), new AssemblyReference(assembly4) },
                                                   new[] { new UsingNamespace(using3), new UsingNamespace(using4) },
                                                   new[] { var3, var4 },
                                                   Enumerable.Empty<VariableType>());

            Assert.That(() => first.Equals(second), Is.False);
        }

        [Test,AutoMoqData]
        public void GethashCode_returns_same_value_for_two_equal_instances(string expression,
                                                                           string assembly1,
                                                                           string assembly2,
                                                                           string using1,
                                                                           string using2,
                                                                           string var1,
                                                                           string var2)
        {
            var first = new ExpressionDescription(expression,
                                                  new[] { new AssemblyReference(assembly1), new AssemblyReference(assembly2) },
                                                  new[] { new UsingNamespace(using1), new UsingNamespace(using2) },
                                                  new[] { var1, var2 },
                                                  Enumerable.Empty<VariableType>());

            var second = new ExpressionDescription(expression,
                                                   new[] { new AssemblyReference(assembly1), new AssemblyReference(assembly2) },
                                                   new[] { new UsingNamespace(using1), new UsingNamespace(using2) },
                                                   new[] { var1, var2 },
                                                   Enumerable.Empty<VariableType>());

            Assert.That(() => first.GetHashCode(), Is.EqualTo(second.GetHashCode()));
        }

        [Test,AutoMoqData]
        public void GethashCode_returns_different_values_for_two_different_instances(string expression1,
                                                                                     string expression2,
                                                                                     string assembly1,
                                                                                     string assembly2,
                                                                                     string assembly3,
                                                                                     string assembly4,
                                                                                     string using1,
                                                                                     string using2,
                                                                                     string using3,
                                                                                     string using4,
                                                                                     string var1,
                                                                                     string var2,
                                                                                     string var3,
                                                                                     string var4)
        {
            var first = new ExpressionDescription(expression1,
                                                  new[] { new AssemblyReference(assembly1), new AssemblyReference(assembly2) },
                                                  new[] { new UsingNamespace(using1), new UsingNamespace(using2) },
                                                  new[] { var1, var2 },
                                                  Enumerable.Empty<VariableType>());

            var second = new ExpressionDescription(expression2,
                                                   new[] { new AssemblyReference(assembly3), new AssemblyReference(assembly4) },
                                                   new[] { new UsingNamespace(using3), new UsingNamespace(using4) },
                                                   new[] { var3, var4 },
                                                   Enumerable.Empty<VariableType>());

            Assert.That(() => first.GetHashCode(), Is.Not.EqualTo(second.GetHashCode()));
        }
    }
}