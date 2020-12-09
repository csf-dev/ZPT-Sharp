using System;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ZptSharp.Autofixture;

namespace ZptSharp.Expressions.PythonExpressions
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class ScriptEngineEvaluatorIntegrationTests
    {
        // Note - the tests in this class are really integration tests, because on its own this
        // class is untestable.  That's because it relies on ScriptEngine which is sealed and
        // cannot be replaced with a test fake.

        [Test, AutoMoqData]
        public async Task EvaluateExpressionAsync_returns_result_from_python_ScriptEngine([MockLogger] ILogger<ScriptEngineEvaluator> logger)
        {
            var scriptEngineProvider = new IronPythonScriptEngineContainer();
            var scriptProvider = new ClassDefinitionScriptFactory();
            var variables = new[]
            {
                new Variable("value", 5),
            };
            var sut = new ScriptEngineEvaluator(scriptEngineProvider, scriptProvider, logger);

            var result = await sut.EvaluateExpressionAsync("value + 2", variables);

            Assert.That(result, Is.EqualTo(7));
        }

        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_throws_if_expression_is_null([MockLogger] ILogger<ScriptEngineEvaluator> logger)
        {
            var scriptEngineProvider = new IronPythonScriptEngineContainer();
            var scriptProvider = new ClassDefinitionScriptFactory();
            var variables = new[]
            {
                new Variable("value", 5),
            };
            var sut = new ScriptEngineEvaluator(scriptEngineProvider, scriptProvider, logger);

            Assert.That(() => sut.EvaluateExpressionAsync(null, variables), Throws.ArgumentNullException);
        }

        [Test, AutoMoqData]
        public void EvaluateExpressionAsync_throws_if_variables_are_null([MockLogger] ILogger<ScriptEngineEvaluator> logger)
        {
            var scriptEngineProvider = new IronPythonScriptEngineContainer();
            var scriptProvider = new ClassDefinitionScriptFactory();
            var sut = new ScriptEngineEvaluator(scriptEngineProvider, scriptProvider, logger);

            Assert.That(() => sut.EvaluateExpressionAsync("1", null), Throws.ArgumentNullException);
        }
    }
}
