using System;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;

namespace ZptSharp.Expressions.PythonExpressions
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class ScriptEngineEvaluatorIntegrationTests
    {
        // Note - the tests in this class are really integration tests, because on its own this
        // class is untestable.  That's because it relies on ScriptEngine which is sealed and
        // cannot be replaced with a test fake.

        [Test]
        public async Task EvaluateExpressionAsync_returns_result_from_python_ScriptEngine()
        {
            var scriptEngineProvider = new IronPythonScriptEngineContainer();
            var scriptProvider = new ClassDefinitionScriptFactory();
            var variables = new[]
            {
                new Variable("value", 5),
            };
            var sut = new ScriptEngineEvaluator(scriptEngineProvider, scriptProvider, Mock.Of<ILogger<ScriptEngineEvaluator>>());

            var result = await sut.EvaluateExpressionAsync("value + 2", variables);

            Assert.That(result, Is.EqualTo(7));
        }
    }
}
