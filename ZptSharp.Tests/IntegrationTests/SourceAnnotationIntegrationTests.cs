using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ZptSharp.Config;
using ZptSharp.Util;

namespace ZptSharp.IntegrationTests
{
    [TestFixture, Parallelizable, Category("Integration")]
    public class SourceAnnotationIntegrationTests
    {
        [Test, Description("For the 'expected rendering' file, rendering the corresponding 'source document' file via ZptSharp should produce the same output.")]
        public async Task Each_output_file_should_render_as_expected([ValueSource(nameof(GetExpectedOutputFiles))] string expectedPath)
        {
            var config = GetConfig();
            var result = await IntegrationTester.PerformIntegrationTest(expectedPath, config: config, logLevel: Microsoft.Extensions.Logging.LogLevel.Debug);
            Assert.That(result, Has.MatchingExpectedAndActualRenderingsExceptDirectorySeparators);
        }

        public static IEnumerable<string> GetExpectedOutputFiles()
            => TestFiles.GetIntegrationTestExpectedFiles<SourceAnnotationIntegrationTests>();

        RenderingConfig GetConfig()
        {
            var builder = RenderingConfig.CreateBuilder();
            builder.ContextBuilder = (c, s) =>
            {
                c.AddToRootContext("tests", new { input = new TemplateDirectory(TestFiles.GetIntegrationTestSourceDirectory<SourceAnnotationIntegrationTests>()) });
            };
            builder.IncludeSourceAnnotation = true;
            builder.SourceAnnotationBasePath = TestFiles.GetPath(nameof(SourceAnnotationIntegrationTests));
            return builder.GetConfig();
        }
    }
}
