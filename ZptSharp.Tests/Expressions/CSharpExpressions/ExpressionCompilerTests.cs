using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace ZptSharp.Expressions.CSharpExpressions
{
    [TestFixture, Parallelizable]
    public class ExpressionCompilerTests
    {
        [Test]
        public async Task GetExpressionAsync_can_get_an_expression_which_uses_two_variables()
        {
            var sut = new ExpressionCompiler(NullLogger<ExpressionCompiler>.Instance, new ScriptBodyFactory());
            var id = new ExpressionDescription("first + second",
                                              Enumerable.Empty<AssemblyReference>(),
                                              Enumerable.Empty<UsingNamespace>(),
                                              new[] { "first", "second" },
                                              Enumerable.Empty<VariableType>());
            var expression = await sut.GetExpressionAsync(id);

            var vars = new Dictionary<string,object>
            {
                { "first", 4 },
                { "second", 6 },
            };

            Assert.That(() => expression(vars), Is.EqualTo(10));
        }

        [Test]
        public async Task GetExpressionAsync_can_get_an_expression_which_uses_an_extension_method()
        {
            var sut = new ExpressionCompiler(NullLogger<ExpressionCompiler>.Instance, new ScriptBodyFactory());
            var id = new ExpressionDescription("items.First()",
                                               Enumerable.Empty<AssemblyReference>(),
                                               new[] { new UsingNamespace("System.Linq") },
                                               new[] { "items" },
                                               new [] { new VariableType("items", "string[]") });
            var expression = await sut.GetExpressionAsync(id);

            var vars = new Dictionary<string,object>
            {
                { "items", new [] { "foo", "bar", "baz" } },
            };

            Assert.That(() => expression(vars), Is.EqualTo("foo"));
        }
   }
}