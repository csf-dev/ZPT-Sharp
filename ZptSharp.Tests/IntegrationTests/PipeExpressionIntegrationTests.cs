using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Util;

namespace ZptSharp.IntegrationTests
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class PipeExpressionIntegrationTests
    {
        [Test, Description("For the 'expected rendering' file, rendering the corresponding 'source document' file via ZptSharp should produce the same output.")]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath, model: GetModel(), config: GetConfig());
            Assert.That(result, Has.MatchingExpectedAndActualRenderings);
        }

        public static IEnumerable<string> GetExpectedOutputFiles() => TestFiles.GetIntegrationTestExpectedFiles<PipeExpressionIntegrationTests>();

        object GetModel() => new object();

        RenderingConfig GetConfig()
        {
            var builder = RenderingConfig.CreateBuilder();

            builder.ContextBuilder = (c, s) =>
            {
                c.AddToRootContext("options", IntegrationTestDataProvider.GetPipeOptionsObject());
            };

            return builder.GetConfig();
        }

        TemplateDirectory GetSourceTemplateDirectory()
            => new TemplateDirectory(TestFiles.GetIntegrationTestSourceDirectory<PipeExpressionIntegrationTests>());
    }
}
